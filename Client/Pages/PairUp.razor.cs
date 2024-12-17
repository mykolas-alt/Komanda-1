using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;
using System;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Drawing;

namespace Projektas.Client.Pages {
    public partial class PairUp : ComponentBase {
        [Inject]
        public IPairUpService PairUpService {get; set;}

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get; set;}
        
        [Inject]
        public required ITimerService TimerService {get; set;}

        
        public string gameScreen = "main";
        private GameDifficulty Difficulty {get; set;} = GameDifficulty.Normal;

        public List<Card> cards {get; set;}
        public Card? firstSelectedCard {get; private set;}
        public Card? secondSelectedCard {get; private set;}
        public bool missMatch {get; private set;}
        public int matchedPairsCount {get; private set;}
        public int mistakes {get; private set;}
        public string gridStyle {get; private set;}
        public bool changeIcon {get; private set;}
        public int ElapsedTime = 0;

        private UserScoreDto<PairUpData>? highscore {get; set;}
        private bool highscoreChecked = false;
        public List<UserScoreDto<PairUpData>>? topScores {get; private set;}

        public string? username = null;

        string[] cardIcons = new string[] {
            "\u2660",  // Spade: ♠
            "\u2663",  // Club: ♣
            "\u25A1",  // Square: □ 
            "\u25B3",  // Triangle: △ 
            "\u2605",  // Star: ★
            "\u2609",  // Sun: ☉
            "\u2602",  // Umbrella: ☂
            "\u263A",  // Smiley Face: ☺
            "\u260E",  // Telephone: ☎ 
            "\u2708",  // Airplane: ✈
            "\u2709",  // Envelope: ✉
            "\u266B",  // Music Note: ♫
            "\u25CB",  // Circle: ○ 
            "\u263D",  // Crescent Moon: ☽
            "\u2714",  // Checkmark: ✔ 
            "\u273F"   // Flower: ✿
        };

        public void ChangeScreen(string mode) {
            gameScreen = mode;
        }

        public async void ChangeDifficulty(string mode) {
            switch(mode) {
                case "Easy":
                    Difficulty = GameDifficulty.Easy;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await PairUpService.GetTopScoresAsync(Difficulty, topCount: 10);
			        StateHasChanged();
                    break;
                case "Normal":
                    Difficulty = GameDifficulty.Normal;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await PairUpService.GetTopScoresAsync(Difficulty, topCount: 10);
			        StateHasChanged();
                    break;
                case "Hard":
                    Difficulty = GameDifficulty.Hard;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await PairUpService.GetTopScoresAsync(Difficulty, topCount: 10);
			        StateHasChanged();
                    break;
            }
        }

        static public string FormatTime(int totalSeconds) {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        private async Task FetchHighscoreAsync() {
            try {
                highscore = await PairUpService.GetUserHighscoreAsync(username, Difficulty);
            } catch {
                highscore = null;
            } finally {
                highscoreChecked = true;
            }
        }

        protected override async Task OnInitializedAsync() {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
            
            await LoadUsernameAsync();
            if(username != null) {
                await FetchHighscoreAsync();
            }
            topScores = await PairUpService.GetTopScoresAsync(Difficulty, topCount: 10);
        }

        private async Task LoadUsernameAsync() {
            username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            StateHasChanged();
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
            await InvokeAsync(LoadUsernameAsync);
            StateHasChanged();
        }

        public void StartGame() {
            TimerService.OnTick += TimerTick;
            ResetGame();
            gameScreen = "started";
        }

        public void ResetGame() {
            ElapsedTime = 0;
            TimerService.Stop();
            mistakes = 0;
            matchedPairsCount = 0;
            firstSelectedCard = null;
            secondSelectedCard = null;
            missMatch = false;
            int count = 0;

            switch(Difficulty) {
                case GameDifficulty.Easy:
                    gridStyle = "grid-template-columns: repeat(4, 100px);";
                    changeIcon = false;
                    count = 8;
                    break;
                case GameDifficulty.Normal:
                    gridStyle = "grid-template-columns: repeat(8, 80px);";
                    changeIcon = true;
                    count = 16;
                    break;
                case GameDifficulty.Hard:
                    gridStyle = "grid-template-columns: repeat(12, 70px);";
                    changeIcon = false;
                    count = 24;
                    break;
            }

            cards = GenerateCardDeck(count).OrderBy(c => Guid.NewGuid()).ToList(); // shuffle cards
            TimerService.Start(1800);
        }

        public void TimerTick() {
            ElapsedTime++;
            if(TimerService.RemainingTime == 0) {
                ResetGame();
            }

            InvokeAsync(StateHasChanged);
        }

        private List<Card> GenerateCardDeck(int count) {
            var cardValues = Enumerable.Range(1, count).ToList();
            var allCards = cardValues.Concat(cardValues)
                .Select(value => new Card {Value = (object)value, IsMatched = false, IsSelected = false})
                .ToList();
            return allCards;
        }

        public async Task OnCardSelectedAsync(Card selectedCard) {
            if(gameScreen != "started" || selectedCard.IsMatched || selectedCard == firstSelectedCard || missMatch)
                return;

            selectedCard.IsSelected = true;

            if(firstSelectedCard == null) {
                firstSelectedCard = selectedCard;
            } else if(secondSelectedCard == null) {
                secondSelectedCard = selectedCard;

                if((int)firstSelectedCard.Value == (int)secondSelectedCard.Value) {
                    firstSelectedCard.IsMatched = true;
                    secondSelectedCard.IsMatched = true;
                    matchedPairsCount++;
                    firstSelectedCard = null;
                    secondSelectedCard = null;

                    if(cards.All(c => c.IsMatched)) {
                        TimerService.Stop();
                        if(username != null) {
                            await PairUpService.SaveScoreAsync(username, ElapsedTime, mistakes, Difficulty);
                            await FetchHighscoreAsync();
                        }
                        topScores = await PairUpService.GetTopScoresAsync(Difficulty, topCount: 10);
                        gameScreen = "ended";
                    }
                } else {
                    mistakes++;
                    var first = firstSelectedCard;
                    var second = secondSelectedCard;
                    missMatch = true;
                    await Task.Delay(1000).ContinueWith(_ => {
                        first.IsSelected = false;
                        second.IsSelected = false;
                        firstSelectedCard = null;
                        secondSelectedCard = null;
                        missMatch = false;
                        InvokeAsync(StateHasChanged);
                    });
                }
            }

            InvokeAsync(StateHasChanged);
        }

        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
        }

        public class Card {
            public required object Value {get; set;}
            public bool IsMatched {get; set;}
            public bool IsSelected {get; set;}
        }
    }
}
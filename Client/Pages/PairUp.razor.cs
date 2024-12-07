using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Client.Services;
using Projektas.Shared.Enums;
using System;

namespace Projektas.Client.Pages {
    public partial class PairUp : ComponentBase {

        [Inject]
        public required ITimerService TimerService { get; set; }
        public List<Card> cards {get; set;}
        public Card? firstSelectedCard {get; private set;}
        public Card? secondSelectedCard {get; private set;}
        public bool isGameActive {get; set;}
        public bool missMatch {get; private set;}
        public int matchedPairsCount {get; private set;}
        public int mistakes {get; private set;}
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
        private Difficulty CurrentDifficulty { get; set; }
        public string gridStyle {get; private set;}
        public bool changeIcon {get; private set;}
        public int ElapsedTime = 0;

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

        public PairUp() {

        }

        [Inject]
        public IPairUpService PairUpService {get; set;}

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get; set;}


        protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
            TimerService.OnTick += TimerTick;
            CurrentDifficulty = Difficulty.Medium;
            ResetGame();
            await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        public void OnDifficultyChanged(ChangeEventArgs e)
        {
            if (Enum.TryParse(e.Value?.ToString(), true, out Difficulty parsedDifficulty))
            {
                CurrentDifficulty = parsedDifficulty;
            }
        }

        public void ResetGame() {
            ElapsedTime = 0;
            TimerService.Stop();
            mistakes = 0;
            matchedPairsCount = 0;
            firstSelectedCard = null;
            secondSelectedCard = null;
            missMatch = false;
            isGameActive = true;
            int count = 0;

            switch (CurrentDifficulty)
            {
                case Difficulty.Easy:
                    {
                        gridStyle = "grid-template-columns: repeat(4, 81px);";
                        changeIcon = false;
                        count = 8;
                        break;
                    }
                case Difficulty.Medium:
                    {
                        gridStyle = "grid-template-columns: repeat(8, 81px);";
                        changeIcon = true;
                        count = 16;
                        break;
                    }
                case Difficulty.Hard:
                    {
                        gridStyle = "grid-template-columns: repeat(8, 81px);";
                        changeIcon = false;
                        count = 24;
                        break;
                    }
            }


            cards = GenerateCardDeck(count).OrderBy(c => Guid.NewGuid()).ToList(); // shuffle cards
            TimerService.Start(1800);

        }

        public void TimerTick()
        {
            ElapsedTime++;
            if (TimerService.RemainingTime == 0)
            {
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

        public void OnCardSelected(Card selectedCard) {
            if(!isGameActive || selectedCard.IsMatched || selectedCard == firstSelectedCard || missMatch)
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
                        if (username != null) {
                            PairUpService.SaveScoreAsync(username, 0, mistakes);
                        }
                        isGameActive = false;
                    }
                } else {
                    if(firstSelectedCard.HasBeenSeen || secondSelectedCard.HasBeenSeen)
                    {
                        mistakes++;
                    }
                    firstSelectedCard.HasBeenSeen = secondSelectedCard.HasBeenSeen = true;

                    var first = firstSelectedCard;
                    var second = secondSelectedCard;
                    missMatch = true;
                    Task.Delay(1000).ContinueWith(_ => {
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
            public bool HasBeenSeen = false;
            public bool IsMatched {get; set;}
            public bool IsSelected {get; set;}
        }
    }
}

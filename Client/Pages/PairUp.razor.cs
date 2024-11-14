using Microsoft.AspNetCore.Components;

namespace Projektas.Client.Pages {
    public partial class PairUp : ComponentBase {
        private List<Card> cards;
        private Card? firstSelectedCard;
        private Card? secondSelectedCard;
        private bool isGameActive;
        private bool missMatch;
        private int matchedPairsCount;
        private int attempts;
        private bool isHardMode;
        private string gridStyle;
        private bool changeIcon;

        string[] cardIcons=new string[] {
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
            ResetGame();
        }

        private void OnDifficultyChanged(ChangeEventArgs e) {
            isHardMode=e.Value?.ToString()=="Hard";
            Console.WriteLine(isHardMode);
        }

        private void ResetGame() {
            attempts=0;
            matchedPairsCount=0;
            firstSelectedCard=null;
            secondSelectedCard=null;
            missMatch=false;
            isGameActive=true;
            int count;

            if (isHardMode) {
                gridStyle="grid-template-columns: repeat(8, 81px);";
                changeIcon=true;
                count=16;
            } else {
                gridStyle="grid-template-columns: repeat(4, 81px);";
                changeIcon=false;
                count=8;
            }
            
            cards=GenerateCardDeck(count).OrderBy(c => Guid.NewGuid()).ToList(); // shuffle cards
        }

        private List<Card> GenerateCardDeck(int count) {
            var cardValues=Enumerable.Range(1, count).ToList(); 
            var allCards=cardValues.Concat(cardValues)
                .Select(value => new Card {Value=(object)value,IsMatched=false,IsSelected=false})
                .ToList();
            return allCards;
        }

        private void OnCardSelected(Card selectedCard) {
            if (!isGameActive || selectedCard.IsMatched || selectedCard==firstSelectedCard || missMatch)
                return;

            selectedCard.IsSelected=true;

            if (firstSelectedCard==null) {
                firstSelectedCard=selectedCard;
            } else if (secondSelectedCard==null) {
                secondSelectedCard=selectedCard;
                attempts++;

                if ((int)firstSelectedCard.Value==(int)secondSelectedCard.Value) {
                    firstSelectedCard.IsMatched=true;
                    secondSelectedCard.IsMatched=true;
                    matchedPairsCount++;
                    firstSelectedCard=null;
                    secondSelectedCard=null;

                    if (cards.All(c => c.IsMatched)) {
                       isGameActive=false;
                    }
                } else {
                    var first=firstSelectedCard;
                    var second=secondSelectedCard;
                    missMatch=true;
                    Task.Delay(1000).ContinueWith(_ => {
                        first.IsSelected=false;
                        second.IsSelected=false;
                        firstSelectedCard=null;
                        secondSelectedCard=null;
                        missMatch=false;
                        StateHasChanged();
                    });
                }
            }

            StateHasChanged();
        }
        public class Card {
            public required object Value {get;set;}
            public bool IsMatched {get;set;}
            public bool IsSelected {get;set;}
        }
    }
}

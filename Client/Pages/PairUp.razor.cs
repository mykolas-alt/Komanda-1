using Microsoft.AspNetCore.Components;

namespace Projektas.Client.Pages
{
    public partial class PairUp : ComponentBase
    {
        private List<Card> cards;
        private Card? firstSelectedCard;
        private Card? secondSelectedCard;
        private bool isGameActive;
        private bool missMatch;
        private int matchedPairsCount;
        private int attempts;

        public PairUp()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            attempts = 0;
            matchedPairsCount = 0;
            firstSelectedCard = null;
            secondSelectedCard = null;
            missMatch = false;
            isGameActive = true;

            cards = GenerateCardDeck().OrderBy(c => Guid.NewGuid()).ToList(); // shuffle cards
        }
        private List<Card> GenerateCardDeck()
        {
            var cardValues = Enumerable.Range(1, 8).ToList(); 
            var allCards = cardValues.Concat(cardValues)
                                     .Select(value => new Card { Value = (object)value, IsMatched = false, IsSelected = false })
                                     .ToList();
            return allCards;
        }
        private void OnCardSelected(Card selectedCard)
        {
            if (!isGameActive || selectedCard.IsMatched || selectedCard == firstSelectedCard || missMatch)
                return;

            selectedCard.IsSelected = true;

            if (firstSelectedCard == null)
            {
                firstSelectedCard = selectedCard;
            }
            else if (secondSelectedCard == null)
            {
                secondSelectedCard = selectedCard;
                attempts++;

                if ((int)firstSelectedCard.Value == (int)secondSelectedCard.Value) // unboxing
                {
                    firstSelectedCard.IsMatched = true;
                    secondSelectedCard.IsMatched = true;
                    matchedPairsCount++;
                    firstSelectedCard = null;
                    secondSelectedCard = null;

                    if (cards.All(c => c.IsMatched))
                    {
                       isGameActive = false;
                    }
                }
                else
                {
                    var first = firstSelectedCard;
                    var second = secondSelectedCard;
                    missMatch = true;
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        first.IsSelected = false;
                        second.IsSelected = false;
                        firstSelectedCard = null;
                        secondSelectedCard = null;
                        missMatch = false;
                        StateHasChanged();
                    });
                }
            }

            StateHasChanged();
        }
        public class Card
        {
            public required object Value { get; set; } //boxing
            public bool IsMatched { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}

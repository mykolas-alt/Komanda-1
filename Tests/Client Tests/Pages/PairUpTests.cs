using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Interfaces;
using Projektas.Client.Pages;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Tests.Client_Tests.Pages {
    public class PairUpTests : TestContext {
        private readonly Mock<IPairUpService> _mockPairUpService;
		private readonly Mock<IAccountAuthStateProvider> _mockAuthStateProvider;
        private readonly Mock<ITimerService> _mockTimerService;

        public PairUpTests() {
			_mockPairUpService = new Mock<IPairUpService>();
			_mockAuthStateProvider = new Mock<IAccountAuthStateProvider>();
            _mockTimerService = new Mock<ITimerService>();


            Services.AddSingleton(_mockPairUpService.Object);
			Services.AddSingleton(_mockAuthStateProvider.Object);
            Services.AddSingleton(_mockTimerService.Object);

			_mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
		}

        [Fact]
        public void StartGame_ShouldStartGame() {
            var cut = RenderComponent<PairUp>();
            cut.Instance.StartGame();

            Assert.Equal("started", cut.Instance.gameScreen);
            Assert.Equal(0, cut.Instance.mistakes);
            Assert.Equal(0, cut.Instance.matchedPairsCount);
            Assert.Null(cut.Instance.firstSelectedCard);
            Assert.Null(cut.Instance.secondSelectedCard);
            Assert.False(cut.Instance.missMatch);
            Assert.Equal("grid-template-columns: repeat(8, 80px);", cut.Instance.gridStyle);
            Assert.True(cut.Instance.changeIcon);
        }

        [Fact]
        public async Task OnCardSelected_ShouldMatchCards_WhenValuesAreEqual()
        {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.StartGame();
            var card1 = new PairUp.Card { Value = 1, IsMatched = false, IsSelected = false };
            var card2 = new PairUp.Card { Value = 1, IsMatched = false, IsSelected = false };

            await pairUp.Instance.OnCardSelectedAsync(card1);
            await pairUp.Instance.OnCardSelectedAsync(card2);

            await Task.Delay(1100);

            Assert.Equal(1, pairUp.Instance.matchedPairsCount);
            Assert.Equal(0, pairUp.Instance.mistakes);
        }

        [Fact]
        public async Task OnCardSelected_ShouldNotMatchCards_WhenValuesAreNotEqual()
        {
            var pairUp = RenderComponent<PairUp>();
            var card1 = new PairUp.Card { Value = 1, IsMatched = false, IsSelected = false };
            var card2 = new PairUp.Card { Value = 2, IsMatched = false, IsSelected = false };
            pairUp.Instance.cards = new List<PairUp.Card> { card1, card2 };
            pairUp.Instance.StartGame();


            await pairUp.Instance.OnCardSelectedAsync(card1);
            await pairUp.Instance.OnCardSelectedAsync(card2); 

            await Task.Delay(1100);

            Assert.Null(pairUp.Instance.firstSelectedCard);
            Assert.Null(pairUp.Instance.secondSelectedCard);
            Assert.Equal(1, pairUp.Instance.mistakes);
        }

        [Fact]
        public async Task OnCardSelected_ShouldNotSelectCard_WhenGameIsInactive() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card = new PairUp.Card {Value = 1, IsMatched = false, IsSelected = false};

            await pairUp.Instance.OnCardSelectedAsync(card);

            Assert.False(card.IsSelected);
        }

        [Fact]
        public async Task OnCardSelected_ShouldNotSelectCard_WhenCardIsAlreadyMatched() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.StartGame();
            var card = new PairUp.Card {Value = 1, IsMatched = true};

            await pairUp.Instance.OnCardSelectedAsync(card);

            Assert.False(card.IsSelected);
        }

        [Fact]
        public async Task OnCardSelected_ShouldNotSelectCard_WhenCardIsSameAsFirstSelectedCard()
        {
            var pairUp = RenderComponent<PairUp>();
            var card = new PairUp.Card { Value = 1, IsMatched = false, IsSelected = false };
            pairUp.Instance.cards = new List<PairUp.Card> { card, new PairUp.Card { Value = 2, IsMatched = false, IsSelected = false } };
            pairUp.Instance.StartGame();

            await pairUp.Instance.OnCardSelectedAsync(card);
            await pairUp.Instance.OnCardSelectedAsync(card);

            Assert.True(card.IsSelected);
            Assert.Null(pairUp.Instance.secondSelectedCard);
        }

        [Fact]
        public async Task OnCardSelected_ShouldHandleMismatch_WhenCardsDoNotMatch()
        {
            var pairUp = RenderComponent<PairUp>();
            var card1 = new PairUp.Card { Value = 1, IsMatched = false, IsSelected = false };
            var card2 = new PairUp.Card { Value = 2, IsMatched = false, IsSelected = false };
            pairUp.Instance.cards = new List<PairUp.Card> { card1, card2 };
            pairUp.Instance.StartGame();

            await pairUp.Instance.OnCardSelectedAsync(card1);
            await pairUp.Instance.OnCardSelectedAsync(card2);

            await Task.Delay(1100);

            Assert.Equal(0, pairUp.Instance.matchedPairsCount);
            Assert.Equal(1, pairUp.Instance.mistakes);
        }

        [Fact]
        public async Task OnCardSelected_ShouldEndGame_WhenAllCardsAreMatched() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.StartGame();
            var cards = pairUp.Instance.cards;

            var cardPairs = cards.SelectMany((card, index) => cards.Skip(index + 1), (card1, card2) => new {card1, card2})
                .Where(pair => (int)pair.card1.Value == (int)pair.card2.Value);

            foreach(var pair in cardPairs) {
                await pairUp.Instance.OnCardSelectedAsync(pair.card1);
                await pairUp.Instance.OnCardSelectedAsync(pair.card2);
            }
            Assert.Equal(16, pairUp.Instance.matchedPairsCount);
            Assert.Equal("ended", pairUp.Instance.gameScreen);
        }
    }
}

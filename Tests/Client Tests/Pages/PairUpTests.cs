using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Interfaces;
using Projektas.Client.Pages;
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

            // setup
            _mockPairUpService.Setup(s => s.SaveScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
			_mockPairUpService.Setup(s => s.GetUserHighscoreAsync(It.IsAny<string>())).ReturnsAsync(new UserScoreDto<PairUpData> {
                Username = "User",
                GameData = new PairUpData {
                    TimeInSeconds = 30,
                    Fails = 10
                }
            });
			_mockPairUpService.Setup(s => s.GetTopScoresAsync(It.IsAny<int>())).ReturnsAsync(new List<UserScoreDto<PairUpData>> {
				new UserScoreDto<PairUpData> {
                    Username = "User1",
                    GameData = new PairUpData {
                        TimeInSeconds = 30,
                        Fails = 1
                    }
                },
				new UserScoreDto<PairUpData> {
                    Username = "User2",
                    GameData = new PairUpData {
                        TimeInSeconds = 30,
                        Fails = 5
                    }
                },
				new UserScoreDto<PairUpData> {
                    Username = "User3",
                    GameData = new PairUpData {
                        TimeInSeconds = 60,
                        Fails = 5
                    }
                }
			});

			_mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
		}

        [Fact]
        public void ResetGame_ShouldStartGame() {
            var cut = RenderComponent<PairUp>();

            Assert.Equal(0, cut.Instance.mistakes);
            Assert.Equal(0, cut.Instance.matchedPairsCount);
            Assert.Null(cut.Instance.firstSelectedCard);
            Assert.Null(cut.Instance.secondSelectedCard);
            Assert.False(cut.Instance.missMatch);
            Assert.True(cut.Instance.isGameActive);
            Assert.Equal("grid-template-columns: repeat(4, 81px);", cut.Instance.gridStyle);
            Assert.False(cut.Instance.changeIcon);
        }

        [Fact]
        public void OnDifficultyChanged_ShouldSetHardMode() {
            var cut = RenderComponent<PairUp>();
            var changeEventArgs = new ChangeEventArgs {Value = "Hard"};

            cut.Instance.OnDifficultyChanged(changeEventArgs);

            Assert.True(cut.Instance.isHardMode);
        }

        [Fact]
        public void ResetGame_ShouldStartGameInHardMode() {
            var cut = RenderComponent<PairUp>();
            var changeEventArgs = new ChangeEventArgs {Value = "Hard"};
            cut.Instance.OnDifficultyChanged(changeEventArgs);

            cut.Instance.ResetGame();

            Assert.True(cut.Instance.isHardMode);

            Assert.Equal(0, cut.Instance.mistakes);
            Assert.Equal(0, cut.Instance.matchedPairsCount);
            Assert.Null(cut.Instance.firstSelectedCard);
            Assert.Null(cut.Instance.secondSelectedCard);
            Assert.False(cut.Instance.missMatch);
            Assert.True(cut.Instance.isGameActive);

            Assert.Equal("grid-template-columns: repeat(8, 81px);", cut.Instance.gridStyle);
            Assert.True(cut.Instance.changeIcon);
        }

        [Fact]
        public void OnCardSelected_ShouldMatchCards_WhenValuesAreEqual() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card1 = new PairUp.Card {Value = 1};
            var card2 = new PairUp.Card {Value = 1};

            pairUp.Instance.OnCardSelected(card1);
            pairUp.Instance.OnCardSelected(card2);

            Assert.True(card1.IsMatched);
            Assert.True(card2.IsMatched);
            Assert.Null(pairUp.Instance.firstSelectedCard);
            Assert.Null(pairUp.Instance.secondSelectedCard);
            Assert.Equal(1, pairUp.Instance.matchedPairsCount);
            Assert.Equal(0, pairUp.Instance.mistakes);
        }

        [Fact]
        public void OnCardSelected_ShouldNotMatchCards_WhenValuesAreNotEqual() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card1 = new PairUp.Card {Value = 1};
            var card2 = new PairUp.Card {Value = 2};

            pairUp.Instance.OnCardSelected(card1);
            pairUp.Instance.OnCardSelected(card2);

            Assert.False(card1.IsMatched);
            Assert.False(card2.IsMatched);
            Assert.NotNull(pairUp.Instance.firstSelectedCard);
            Assert.NotNull(pairUp.Instance.secondSelectedCard);
            Assert.Equal(0, pairUp.Instance.matchedPairsCount);
        }

        [Fact]
        public void OnCardSelected_ShouldNotSelectCard_WhenGameIsInactive() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            pairUp.Instance.isGameActive = false;
            var card = new PairUp.Card {Value = 1};

            pairUp.Instance.OnCardSelected(card);

            Assert.False(card.IsSelected);
        }

        [Fact]
        public void OnCardSelected_ShouldNotSelectCard_WhenCardIsAlreadyMatched() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card = new PairUp.Card {Value = 1, IsMatched = true};

            pairUp.Instance.OnCardSelected(card);

            Assert.False(card.IsSelected);
        }

        [Fact]
        public void OnCardSelected_ShouldNotSelectCard_WhenCardIsSameAsFirstSelectedCard() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card = new PairUp.Card {Value = 1};
            pairUp.Instance.OnCardSelected(card);

            pairUp.Instance.OnCardSelected(card);

            Assert.True(card.IsSelected);
            Assert.Equal(card, pairUp.Instance.firstSelectedCard);
            Assert.Null(pairUp.Instance.secondSelectedCard);
        }

        [Fact]
        public void OnCardSelected_ShouldHandleMismatch_WhenCardsDoNotMatch() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var card1 = new PairUp.Card {Value = 1};
            var card2 = new PairUp.Card {Value = 2};

            pairUp.Instance.OnCardSelected(card1);
            pairUp.Instance.OnCardSelected(card2);

            Assert.True(card1.IsSelected);
            Assert.True(card2.IsSelected);
            Assert.True(pairUp.Instance.missMatch);
        }

        [Fact]
        public void OnCardSelected_ShouldEndGame_WhenAllCardsAreMatched() {
            var pairUp = RenderComponent<PairUp>();
            pairUp.Instance.ResetGame();
            var cards = pairUp.Instance.cards;

            var cardPairs = cards.SelectMany((card, index) => cards.Skip(index + 1), (card1, card2) => new {card1, card2})
                .Where(pair => (int)pair.card1.Value == (int)pair.card2.Value);

            foreach(var pair in cardPairs) {
                pairUp.Instance.OnCardSelected(pair.card1);
                pairUp.Instance.OnCardSelected(pair.card2);
            }

            Assert.False(pairUp.Instance.isGameActive);
        }
    }
}

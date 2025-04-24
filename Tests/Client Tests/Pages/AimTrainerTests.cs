﻿using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Interfaces;
using Projektas.Client.Pages;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Tests.Client_Tests.Pages {
    public class AimTrainerTests : TestContext {
        private readonly Mock<IAimTrainerService> _mockAimTrainerService;
        private readonly Mock<ITimerService> _mockTimerService;
        private readonly Mock<Random> _mockRandom;
		private readonly Mock<IAccountAuthStateProvider> _mockAuthStateProvider;

        public AimTrainerTests() {
            _mockAimTrainerService = new Mock<IAimTrainerService>();
            _mockTimerService = new Mock<ITimerService>();
            _mockRandom = new Mock<Random>();
            _mockAuthStateProvider = new Mock<IAccountAuthStateProvider>();
            
            Services.AddSingleton(_mockAimTrainerService.Object);
            Services.AddSingleton(_mockTimerService.Object);
            Services.AddSingleton(_mockRandom.Object);
            Services.AddSingleton(_mockAuthStateProvider.Object);

            _mockAuthStateProvider.Setup(s => s.GetUsernameAsync()).ReturnsAsync("TestUser");
        }

        [Fact]
        public void StartGame_ShouldInitializeGameCorrectly() {
            var cut = RenderComponent<AimTrainer>();

            cut.Instance.StartGame();

            Assert.Equal(0, cut.Instance.score);
            _mockTimerService.Verify(t => t.Start(It.IsAny<int>()), Times.Once);
            _mockTimerService.VerifyAdd(t => t.OnTick += It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public void StartGame_ShouldStartInHardMode() {
            var cut = RenderComponent<AimTrainer>();

            cut.Instance.ChangeDifficulty("Hard");
            cut.Instance.StartGame();

            Assert.Equal(0, cut.Instance.score);
            _mockTimerService.Verify(t => t.Start(It.IsAny<int>()), Times.Once);
            _mockTimerService.VerifyAdd(t => t.OnTick += It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public void OnTargetClicked_ShouldIncreaseScore() {
            _mockTimerService.Setup(t => t.RemainingTime).Returns(10);
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.StartGame();

            cut.Instance.OnTargetClicked();

            Assert.Equal(1, cut.Instance.score);
        }

        [Fact]
        public void ChangeDifficulty_ShouldSetDifficultyToNormal()
        {
            var aimTrainer = RenderComponent<AimTrainer>();

            aimTrainer.Instance.ChangeDifficulty("Normal");

            Assert.Equal(GameDifficulty.Normal, aimTrainer.Instance.Difficulty);
        }

        [Fact]
        public void ChangeDifficulty_ShouldSetDifficultyToHard()
        {
            var aimTrainer = RenderComponent<AimTrainer>();

            aimTrainer.Instance.ChangeDifficulty("Hard");

            Assert.Equal(GameDifficulty.Hard, aimTrainer.Instance.Difficulty);
        }

        [Fact]
        public void TimerTick_ShouldEndGame_WhenTimeIsUp() {
            _mockTimerService.Setup(t => t.RemainingTime).Returns(0);
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.StartGame();

            cut.Instance.TimerTick();

            _mockTimerService.VerifyRemove(t => t.OnTick -= It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public void MoveTarget_ShouldChangeTargetPosition() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.StartGame();
            var initialPosition = cut.Instance.TargetPosition;

            cut.Instance.MoveTargetNormal(1000, 400);

            var newPosition = cut.Instance.TargetPosition;
            Assert.NotEqual(initialPosition, newPosition);
        }

        [Fact]
        public void MoveTarget_ShouldChangesPositionCorrectly() {
            _mockRandom.Setup(r => r.Next(It.IsAny<int>())).Returns(1);
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.ChangeDifficulty("Hard");
            cut.Instance.StartGame();

            cut.Instance.MoveTargetNormal(1000, 400);

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(1, cut.Instance.moveDirection);
            Assert.Equal((1, 0), targetPosition);
        }

        [Fact]
        public void MoveTarget_ChangesTargetPosition_Left() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 10);
            cut.Instance.moveDirection = 0; // left

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(9, targetPosition.x);
            Assert.Equal(10, targetPosition.y);
        }

        [Fact]
        public void MoveTarget_ChangesTargetPosition_Right() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 10);
            cut.Instance.moveDirection = 1; // right

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(11, targetPosition.x);
            Assert.Equal(10, targetPosition.y);
        }

        [Fact]
        public void MoveTarget_ChangesTargetPosition_Up() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 10);
            cut.Instance.moveDirection = 2; // up

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(10, targetPosition.x);
            Assert.Equal(9, targetPosition.y);
        }

        [Fact]
        public void MoveTarget_ChangesTargetPosition_Down() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 10);
            cut.Instance.moveDirection = 3; // down

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(10, targetPosition.x);
            Assert.Equal(11, targetPosition.y);
        }

        [Fact]
        public void MoveTarget_ChangesDirectionAtBoundary_Left() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (4, 10);
            cut.Instance.moveDirection = 0; // left

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(3, targetPosition.x);
            Assert.Equal(10, targetPosition.y);
            Assert.Equal(1, cut.Instance.moveDirection); // direction changed to right
        }

        [Fact]
        public void MoveTarget_ChangesDirectionAtBoundary_Right() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (966, 10);
            cut.Instance.moveDirection = 1; // right

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(967, targetPosition.x);
            Assert.Equal(10, targetPosition.y);
            Assert.Equal(0, cut.Instance.moveDirection); // direction changed to left
        }

        [Fact]
        public void MoveTarget_ChangesDirectionAtBoundary_Up() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 4);
            cut.Instance.moveDirection = 2; // up

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(10, targetPosition.x);
            Assert.Equal(3, targetPosition.y);
            Assert.Equal(3, cut.Instance.moveDirection); // direction changed to down
        }

        [Fact]
        public void MoveTarget_ChangesDirectionAtBoundary_Down() {
            var cut = RenderComponent<AimTrainer>();
            cut.Instance.TargetPosition = (10, 366);
            cut.Instance.moveDirection = 3; // down

            cut.InvokeAsync(() => cut.Instance.MoveTargetNormal(1000, 400));

            var targetPosition = cut.Instance.TargetPosition;
            Assert.Equal(10, targetPosition.x);
            Assert.Equal(367, targetPosition.y);
            Assert.Equal(2, cut.Instance.moveDirection); // direction changed to up
        }
    }
}

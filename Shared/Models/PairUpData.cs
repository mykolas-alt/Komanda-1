using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;
using System;

namespace Projektas.Shared.Models {
    public class PairUpData : IGame, IGameWithDifficulty {
        public int TimeInSeconds {get; set;}
        public int Fails {get; set;}
        public GameDifficulty Difficulty {get; set;}
        public string GetFormattedTimeSpent()
        {
            int minutes = TimeInSeconds / 60;
            int seconds = TimeInSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}

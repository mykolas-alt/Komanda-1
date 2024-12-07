﻿using Projektas.Shared.Interfaces;

namespace Projektas.Shared.Models {
    public class UserScoreDto<T> where T : IGame {
        public string Username {get; set;} = "";
        public DateTime Timestamp {get; set;}
        public T GameData {get; set;}
    }
}

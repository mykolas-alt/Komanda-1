﻿using Projektas.Shared.Models;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Server.Interfaces;

namespace Projektas.Server.Services.MathGame {
    public class MathGameScoreboardService : IComparer<int>, IMathGameScoreboardService {
        private readonly IUserRepository _userRepository;

        public MathGameScoreboardService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            await _userRepository.AddMathGameScoreToUserAsync(data.Username,data.Score);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _userRepository.GetMathGameHighscoreFromUserAsync(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _userRepository.GetAllMathGameScoresAsync();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for (int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }

        public int Compare(int a,int b) {
            return b.CompareTo(a);
        }
    }
}

﻿using Projektas.Shared.Models;
using Projektas.Server.Interfaces;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame
{
    public class MathGameScoreboardService : IComparer<int>, IMathGameScoreboardService
    {
        private readonly IScoreRepository _scoreRepository;

        public MathGameScoreboardService(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<MathGameData> data)
        {
            await _scoreRepository.AddScoreToUserAsync<MathGameData>(data.Username, data.GameData, data.Timestamp);
        }

        public async Task<UserScoreDto<MathGameData>> GetUserHighscoreAsync(string username)
        {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<MathGameData>(username);

            return scores.OrderByDescending(s => s.GameData.Scores).FirstOrDefault();
        }

        public async Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount)
        {
            List<UserScoreDto<MathGameData>> userScores = await _scoreRepository.GetAllScoresAsync<MathGameData>();
            List<UserScoreDto<MathGameData>> orderedScores = userScores.Where(s => !s.IsPrivate).OrderByDescending(s => s.GameData.Scores).ToList();
            List<UserScoreDto<MathGameData>> topScores = new List<UserScoreDto<MathGameData>>();

            for (int i = 0; i < topCount && i < orderedScores.Count; i++)
            {
                topScores.Add(orderedScores[i]);
            }

            return topScores;
        }

        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}
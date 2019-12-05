﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Core.Retrievers
{
    public interface IHomePageStatRetriever
    {
        IEnumerable<DerivedPlayerData> TopPlayersOnBlue();
        IEnumerable<DerivedPlayerData> TopPlayersOnRed();
        int GamesPlayed();
        int[] RedVsBlue();
        IEnumerable<DisplayGame> TodaysGames();
        IEnumerable<DerivedPlayerData> TopPlayersByGoalsAgainstPerGameAvg();
        IEnumerable<DerivedPlayerData> TopPlayersByGoalsPerGameAvg();
        IEnumerable<DerivedPlayerData> TopPlayersByWinPct();
        IEnumerable<DerivedPlayerData> TopPlayersByDefenseWinPct();
        IEnumerable<DerivedPlayerData> TopPlayersByOffenseWinPct();
    }
    public class HomePageStatRetriever : IHomePageStatRetriever
    {

        private IEnumerable<DisplayGame> games;
        private IEnumerable<DerivedPlayerData> leaderboard;

        public HomePageStatRetriever(IGameRetriever gameRetriever,ILeaderboards leaderboards)
        {
            games = gameRetriever.GetAllGames();
            leaderboard = leaderboards.GetLeaderboard();
        }
        public int[] RedVsBlue()
        {
            var redWin = games.Where(r => r.RedScore >r.BlueScore).Count();
            var blueWin = games.Where(r => r.BlueScore >r.RedScore).Count();

            return new int[2]{ redWin, blueWin};
        }
        public IEnumerable<DerivedPlayerData> TopPlayersByWinPct()
        {
            var topPlayers = leaderboard.OrderByDescending(r=>r.WinPercentage);
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<DerivedPlayerData> TopPlayersByGoalsPerGameAvg()
        {
            var topPlayers = leaderboard.OrderByDescending(r => r.AverageGoalsPerGame);
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<DerivedPlayerData> TopPlayersByGoalsAgainstPerGameAvg()
        {
            var topPlayers = leaderboard.OrderBy(r => r.AverageGoalsAgainstPerGame);
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<DisplayGame> TodaysGames()
        {
            var today = DateTime.Now.Date;
            return games.Where(r => r.GameTime.Date == today);
        }
        public int GamesPlayed()
        {
            return games.Count();
        }
        public IEnumerable<DerivedPlayerData> TopPlayersOnRed()
        {
            var bestOnRed = leaderboard.OrderByDescending(r => r.RedWinPct).ToList().Take(5);
            return bestOnRed;
        }
        public IEnumerable<DerivedPlayerData> TopPlayersOnBlue()
        {
            var bestOnBlue= leaderboard.OrderByDescending(r => r.BlueWinPct).ToList().Take(5);
            return bestOnBlue;
        }
        public IEnumerable<DerivedPlayerData> TopPlayersByOffenseWinPct()
        {
            return leaderboard.OrderByDescending(r=>r.OffenceWinPct).ToList().Take(5);
        }
        public IEnumerable<DerivedPlayerData> TopPlayersByDefenseWinPct()
        {
            return leaderboard.OrderByDescending(r => r.DefenseWinPct).ToList().Take(5);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Core.Retrievers
{
    public interface IHomePageStatRetriever
    {
        IEnumerable<DerivedData> TopPlayersOnBlue();
        IEnumerable<DerivedData> TopPlayersOnRed();
        int GamesPlayed();
        int[] RedVsBlue();
        IEnumerable<DisplayGame> TodaysGames();
        IEnumerable<DerivedData> TopPlayersByGoalsAgainstPerGameAvg();
        IEnumerable<DerivedData> TopPlayersByGoalsPerGameAvg();
        IEnumerable<DerivedData> TopPlayersByWinPct();
        IEnumerable<DerivedData> TopPlayersByDefenseWinPct();
        IEnumerable<DerivedData> TopPlayersByOffenseWinPct();
    }
    public class HomePageStatRetriever : IHomePageStatRetriever
    {

        private IEnumerable<DisplayGame> games;
        private IEnumerable<DerivedData> leaderboard;

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
        public IEnumerable<DerivedData> TopPlayersByWinPct()
        {
            var topPlayers = leaderboard.OrderByDescending(r=>r.WinPercentage);
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<DerivedData> TopPlayersByGoalsPerGameAvg()
        {
            var topPlayers = leaderboard.OrderByDescending(r => r.AverageGoalsPerGame);
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<DerivedData> TopPlayersByGoalsAgainstPerGameAvg()
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
        public IEnumerable<DerivedData> TopPlayersOnRed()
        {
            var bestOnRed = leaderboard.OrderByDescending(r => r.RedWinPct).ToList().Take(5);
            return bestOnRed;
        }
        public IEnumerable<DerivedData> TopPlayersOnBlue()
        {
            var bestOnBlue= leaderboard.OrderByDescending(r => r.BlueWinPct).ToList().Take(5);
            return bestOnBlue;
        }
        public IEnumerable<DerivedData> TopPlayersByOffenseWinPct()
        {
            return leaderboard.OrderByDescending(r=>r.OffenceWinPct).ToList().Take(5);
        }
        public IEnumerable<DerivedData> TopPlayersByDefenseWinPct()
        {
            return leaderboard.OrderByDescending(r => r.DefenseWinPct).ToList().Take(5);
        }
    }
}

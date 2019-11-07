using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace FoosStats.Core.Retrievers
{
    public interface IHomePageStatRetriever
    {
        DerivedData BestOnBlue();
        DerivedData BestOnRed();
        int GamesPlayed();
        int[] RedVsBlue();
        IEnumerable<DisplayGame> TodaysGames();
        IEnumerable<DerivedData> TopPlayersByGoalsAgainstPerGameAvg();
        IEnumerable<DerivedData> TopPlayersByGoalsPerGameAvg();
        IEnumerable<DerivedData> TopPlayersByWinPct();
        
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
        public DerivedData BestOnRed()
        {
            var bestOnRed = leaderboard.OrderByDescending(r => r.RedWinPct);
            return bestOnRed.ToList()[0];
        }
        public DerivedData BestOnBlue()
        {
            var bestOnBlue= leaderboard.OrderByDescending(r => r.BlueWinPct);
            return bestOnBlue.ToList()[0];
        }

    }
}

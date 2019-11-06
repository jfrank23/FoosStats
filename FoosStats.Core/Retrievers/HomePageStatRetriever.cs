using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoosStats.Core.Retrievers
{
    public interface IHomePageStatRetriever
    {
        Player BestOnBlue();
        Player BestOnRed();
        int GamesPlayed();
        int[] RedVsBlue();
        void Setup();
        IEnumerable<DisplayGame> TodaysGames();
        IEnumerable<Player> TopPlayersByGoalsAgainstPerGameAvg();
        IEnumerable<Player> TopPlayersByGoalsPerGameAvg();
        IEnumerable<Player> TopPlayersByWinPct();
    }
    public class HomePageStatRetriever : IHomePageStatRetriever
    {
        private readonly IGameRetriever gameRetriever;
        private readonly IPlayerRetriever playerRetriever;
        private IEnumerable<DisplayGame> games;
        private IEnumerable<Player> players;

        public HomePageStatRetriever(IGameRetriever gameRetriever, IPlayerRetriever playerRetriever)
        {
            this.gameRetriever = gameRetriever;
            this.playerRetriever = playerRetriever;
        }
        public int[] RedVsBlue()
        {
            var redWin = games.Where(r => r.RedScore == 10).Count();
            var blueWin = games.Where(r => r.BlueScore == 10).Count();

            return new int[2]{ redWin, blueWin};
        }
        public IEnumerable<Player> TopPlayersByWinPct()
        {
            var topPlayers = players.OrderByDescending(r => ((float)r.GamesWon / r.GamesPlayed));
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<Player> TopPlayersByGoalsPerGameAvg()
        {
            var topPlayers = players.OrderByDescending(r => ((float)r.GoalsFor / r.GamesPlayed));
            return topPlayers.ToList().Take(5);
        }
        public IEnumerable<Player> TopPlayersByGoalsAgainstPerGameAvg()
        {
            var topPlayers = players.OrderBy(r => ((float)r.GoalsAgainst / r.GamesPlayed));
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
        public Player BestOnRed()
        {
            var bestOnRed = new Player();
            float winPct = 0;
            foreach(var player in players)
            {
                var redGames = games.Where(r => r.RedDefense == player.ID || r.RedOffense == player.ID);
                var redGameWins = redGames.Where(r => r.RedScore == 10).Count();
                if((float)redGameWins/redGames.Count() > winPct)
                {
                    bestOnRed = player;
                    winPct = (float)redGameWins / redGames.Count();
                }
            }
            return bestOnRed;
        }
        public Player BestOnBlue()
        {
            var bestOnBlue = new Player();
            float winPct = 0;
            foreach (var player in players)
            {
                var blueGames = games.Where(r => r.BlueDefense == player.ID || r.BlueOffense == player.ID);
                var blueGameWins = blueGames.Where(r => r.BlueScore == 10).Count();
                if ((float)blueGameWins / blueGames.Count() > winPct)
                {
                    bestOnBlue = player;
                    winPct = (float)blueGameWins / blueGames.Count();
                }
            }
            return bestOnBlue;
        }
        public void Setup()
        {
            games = gameRetriever.GetAllGames();
            players = playerRetriever.GetPlayersByName();
        }
    }
}

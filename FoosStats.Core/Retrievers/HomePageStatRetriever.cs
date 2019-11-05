using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoosStats.Core.Retrievers
{
    public interface IHomePageStatRetriever
    {
        int[] RedVsBlue();
        void Setup();
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

        public void Setup()
        {
            games = gameRetriever.GetAllGames();
            players = playerRetriever.GetPlayersByName();
        }
    }
}

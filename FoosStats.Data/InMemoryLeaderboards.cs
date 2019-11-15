using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FoosStats.Core
{
    public class InMemoryLeaderboards : ILeaderboards
    {
        private readonly IEnumerable<Player> players;
        private IEnumerable<DisplayGame> games;
        public IEnumerable<DerivedData> leaderboard;
        public InMemoryLeaderboards(IPlayerRepository playerRepository, IGameRepository gameRepository)
        {
            players = playerRepository.GetPlayersByName();
            games = gameRepository.GetGames();
            MakeLeaderboard();
        }
        public IEnumerable<DerivedData> GetLeaderboard()
        {
            return leaderboard;
        }
        public void MakeLeaderboard()
        {
            var lst =  new List<DerivedData>();
            foreach(var currentPlayer in players)
            {
                if (currentPlayer.GamesPlayed < 3) { continue; }
                lst.Add(new DerivedData
                {
                    player = currentPlayer,
                    WinPercentage= (float)currentPlayer.GamesWon/currentPlayer.GamesPlayed*100,
                    AverageGoalsPerGame = (float)currentPlayer.GoalsFor/currentPlayer.GamesPlayed,
                    AverageGoalsAgainstPerGame = (float)currentPlayer.GoalsAgainst/currentPlayer.GamesPlayed,
                    OffenceWinPct= OffenseWinPct(currentPlayer),
                    DefenseWinPct = DefenseWinPct(currentPlayer),
                    BlueWinPct = BlueSideWinPct(currentPlayer),
                    RedWinPct = RedSideWinPct(currentPlayer)
                });
            }
            leaderboard = lst;
        }
        public void Update()
        {
            MakeLeaderboard();
        }




        public float OffenseWinPct(Player player)
        {
            var redGames = games.Where(r => r.RedOffense == player.ID);
            var blueGames = games.Where(r => r.BlueOffense == player.ID);
            var redWins = redGames.Where(r => r.RedScore == 10).Count();
            var blueWins = blueGames.Where(r => r.BlueScore == 10).Count();
            var offenseWinPct = (float)(redWins + blueWins) / (redGames.Count() + blueGames.Count()) * 100;
            if (float.IsNaN(offenseWinPct)) { offenseWinPct = 0; }
            return offenseWinPct;
        }
        public float DefenseWinPct(Player player)
        {
            var redGames = games.Where(r => r.RedDefense == player.ID);
            var blueGames = games.Where(r => r.BlueDefense == player.ID);
            var redWins = redGames.Where(r => r.RedScore == 10).Count();
            var blueWins = blueGames.Where(r => r.BlueScore == 10).Count();
            var defenseWinPct = (float)(redWins + blueWins) / (redGames.Count() + blueGames.Count()) * 100;
            if (float.IsNaN(defenseWinPct)) { defenseWinPct = 0; }
            return defenseWinPct;
        }
        public float BlueSideWinPct(Player player)
        {
            var blueGames = games.Where(r => r.BlueOffense == player.ID || r.BlueDefense == player.ID);
            var blueWins = blueGames.Where(r => r.BlueScore == 10).Count();
            var blueWinPct = (float)blueWins / blueGames.Count() * 100;
            if (float.IsNaN(blueWinPct)) { blueWinPct = 0; }
            return blueWinPct;
        }
        public float RedSideWinPct(Player player)
        {
            var redGames = games.Where(r => r.RedOffense == player.ID || r.RedDefense == player.ID);
            var redWins = redGames.Where(r => r.RedScore == 10).Count();
            var redWinPct = (float)redWins / redGames.Count() * 100;
            if (float.IsNaN(redWinPct)) { redWinPct = 0; }
            return redWinPct;
        }
    }

}
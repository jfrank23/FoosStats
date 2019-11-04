using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Core.Retrievers
{
    public interface IPlayerDetailHandler
    {
        string BestWinPercentageTeammate();
        float BlueSideWinPct(Player player);
        float DefenseWinPct(Player player);
        IEnumerable<DisplayGame> GetTop3Games(Guid playerID);

        int GoalsAgainstAverageStanding(Player currentPlayer);

        int GoalsForAverageStanding(Player currentPlayer);

        string MostPlayedWith();
        float OffenseWinPct(Player player);
        float RedSideWinPct(Player player);
        void SetupTeammateStats(Guid playerID);

        int WinPercentageStanding(Player currentPlayer);
    }

    public class PlayerDetailHandler : IPlayerDetailHandler
    {
        private IGameRetriever gameRetriever;
        private IPlayerRetriever playerRetriever;
        private IEnumerable<Player> players;
        private IEnumerable<DisplayGame> games;
        private Dictionary<Guid, int[]> teammateStats = new Dictionary<Guid, int[]>();

        public PlayerDetailHandler(IPlayerRetriever playerRetriever, IGameRetriever gameRetriever)
        {
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
        }

        public float DefenseWinPct(Player player)
        {
            var redGames = games.Where(r => r.RedDefense == player.ID);
            var blueGames = games.Where(r => r.BlueDefense == player.ID);
            var redWins = redGames.Where(r => r.RedScore == 10).Count();
            var blueWins = blueGames.Where(r => r.BlueScore == 10).Count();
            var defenseWinPct = (float)(redWins + blueWins)/(redGames.Count() + blueGames.Count()) * 100;
            if (float.IsNaN(defenseWinPct)) { defenseWinPct = 0; }
            return defenseWinPct;
        }
        public float OffenseWinPct(Player player)
        {
            var redGames = games.Where(r => r.RedOffense == player.ID);
            var blueGames = games.Where(r => r.BlueOffense == player.ID);
            var redWins = redGames.Where(r => r.RedScore == 10).Count();
            var blueWins = blueGames.Where(r => r.BlueScore == 10).Count();
            var offenseWinPct = (float)(redWins + blueWins) / (redGames.Count() + blueGames.Count()) *100;
            if (float.IsNaN(offenseWinPct)) { offenseWinPct = 0; }
            return offenseWinPct;
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
        public int WinPercentageStanding(Player currentPlayer)
        {
            int betterStat = 1;
            foreach (var player in players)
            {
                var currentWinPct = (float)currentPlayer.GamesWon / currentPlayer.GamesPlayed;
                var otherWinPct = (float)player.GamesWon / player.GamesPlayed;
                if (currentWinPct < otherWinPct)
                {
                    betterStat += 1;
                }
            }
            return betterStat;
        }

        public int GoalsForAverageStanding(Player currentPlayer)
        {
            int betterStat = 1;
            foreach (var player in players)
            {
                var currentGoalsForPct = (float)currentPlayer.GoalsFor / currentPlayer.GamesPlayed;
                var otherGoalsForPct = (float)player.GoalsFor / player.GamesPlayed;
                if (currentGoalsForPct < otherGoalsForPct)
                {
                    betterStat += 1;
                }
            }
            return betterStat;
        }

        public int GoalsAgainstAverageStanding(Player currentPlayer)
        {
            int betterStat = 1;
            foreach (var player in players)
            {
                var currentGoalsAgainstAvg = (float)currentPlayer.GoalsAgainst / currentPlayer.GamesPlayed;
                var otherGoalsAgainstAvg = (float)player.GoalsAgainst / player.GamesPlayed;
                if (currentGoalsAgainstAvg > otherGoalsAgainstAvg)
                {
                    betterStat += 1;
                }
            }
            return betterStat;
        }

        public IEnumerable<DisplayGame> GetTop3Games(Guid playerID)
        {
            games = gameRetriever.GetAllGames();
            return games
                .Where(r => r.BlueDefense == playerID || r.BlueOffense == playerID || r.RedDefense == playerID || r.RedOffense == playerID)
                .ToList()
                .Take(3);
        }

        public string MostPlayedWith()
        {
            var playedWithMax = 0;
            var mostPlayedWithGuid = Guid.Empty;
            foreach (var teammate in teammateStats.Keys)
            {
                if (teammateStats[teammate][0] > playedWithMax)
                {
                    playedWithMax = teammateStats[teammate][0];
                    mostPlayedWithGuid = teammate;
                }
            }
            var teammatePlayer = playerRetriever.GetPlayerById(mostPlayedWithGuid);
            return teammatePlayer.FirstName + " " + teammatePlayer.LastName;
        }

        public string BestWinPercentageTeammate()
        {
            double wonWithMax = 0.0;
            var mostWonWithGuid = Guid.Empty;
            foreach (var teammate in teammateStats.Keys)
            {
                if (teammateStats[teammate][0] == 0) { continue; }
                if ((float)teammateStats[teammate][1] / teammateStats[teammate][0] > wonWithMax)
                {
                    wonWithMax = (float)teammateStats[teammate][1] / teammateStats[teammate][0];
                    mostWonWithGuid = teammate;
                }
            }
            var teammatePlayer = playerRetriever.GetPlayerById(mostWonWithGuid);
            double winPercentageWithTeammate = wonWithMax * 100;
            return teammatePlayer.FirstName + " " + teammatePlayer.LastName + " (" + winPercentageWithTeammate.ToString("F02") + "%)";
        }

        public void SetupTeammateStats(Guid playerID)
        {
            players = playerRetriever.GetPlayersByName();
            games = gameRetriever.GetAllGames();
            foreach (var player in players)
            {
                teammateStats.Add(player.ID, new int[] { 0, 0 });
            }
            foreach (var game in games)
            {
                if (playerID == game.BlueDefense)
                {
                    if (game.BlueOffense == null) { continue; }
                    teammateStats[game.BlueOffense][0] += 1;
                    if (game.BlueScore == 10) { teammateStats[game.BlueOffense][1] += 1; }
                }
                else if (playerID == game.BlueOffense)
                {
                    if (game.BlueDefense == null) { continue; }
                    teammateStats[game.BlueDefense][0] += 1;
                    if (game.BlueScore == 10) { teammateStats[game.BlueDefense][1] += 1; }
                }
                else if (playerID == game.RedDefense)
                {
                    if (game.RedOffense == null) { continue; }
                    teammateStats[game.RedOffense][0] += 1;
                    if (game.RedScore == 10) { teammateStats[game.RedOffense][1] += 1; }
                }
                else if (playerID == game.RedOffense)
                {
                    if (game.RedDefense == null) { continue; }
                    teammateStats[game.RedDefense][0] += 1;
                    if (game.RedScore == 10) { teammateStats[game.RedDefense][1] += 1; }
                }
            }
            teammateStats.Remove(playerID);
        }
    }
}
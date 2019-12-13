using FoosStats.Core.Repositories;
using FoosStats.Core.Retrievers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Core.PageSpecific
{
    public interface IPlayerDetailRetriever
    {
        string BestWinPercentageTeammate();
        float BlueSideWinPct(Player player);
        float DefenseWinPct(Player player);
        IEnumerable<DisplayGame> GamesPlayedIn(Guid playerID);

        int GoalsAgainstAverageStanding(Player currentPlayer);

        int GoalsForAverageStanding(Player currentPlayer);

        string MostPlayedWith();
        float OffenseWinPct(Player player);
        float RedSideWinPct(Player player);
        void SetupTeammateStats(Guid playerID);

        int WinPercentageStanding(Player currentPlayer);
        int[] AverageEloByPosition(Player player);
    }

    public class PlayerDetailRetriever : IPlayerDetailRetriever
    {
        private IGameRetriever gameRetriever;
        private readonly ITeamRetriever teamRetriever;
        private IPlayerRetriever playerRetriever;
        private IEnumerable<Player> players;
        private IEnumerable<DisplayGame> games;
        private Dictionary<Guid, int[]> teammateStats = new Dictionary<Guid, int[]>();
        private IEnumerable<DerivedPlayerData> leaderboard;
        public PlayerDetailRetriever(IPlayerRetriever playerRetriever, IGameRetriever gameRetriever,ILeaderboards leaderboards, ITeamRetriever teamRetriever)
        {
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
            this.teamRetriever = teamRetriever;
            leaderboard = leaderboards.GetFullLeaderboard();
        }

        public float DefenseWinPct(Player player)
        {
            return leaderboard.FirstOrDefault(r => r.player.ID == player.ID).DefenseWinPct;
            
        }
        public float OffenseWinPct(Player player)
        {
            return leaderboard.FirstOrDefault(r => r.player.ID == player.ID).OffenceWinPct;
        }
        public float BlueSideWinPct(Player player)
        {
            return leaderboard.FirstOrDefault(r => r.player.ID == player.ID).BlueWinPct;
        }
        public float RedSideWinPct(Player player)
        {
            return leaderboard.FirstOrDefault(r => r.player.ID == player.ID).RedWinPct;
        }
        public int WinPercentageStanding(Player currentPlayer)
        {
            var betterStat = leaderboard
                .Where(r => r.WinPercentage > 
                leaderboard.First(u => u.player.ID == currentPlayer.ID).WinPercentage)
                .Count()+1;
            return betterStat;
        }

        public int GoalsForAverageStanding(Player currentPlayer)
        {
            var betterStat = leaderboard
                .Where(r => r.AverageGoalsPerGame >
                leaderboard.First(u => u.player.ID == currentPlayer.ID).AverageGoalsPerGame)
                .Count() + 1;
            return betterStat;
        }

        public int GoalsAgainstAverageStanding(Player currentPlayer)
        {
            var betterStat = leaderboard
                .Where(r => r.AverageGoalsAgainstPerGame <
                leaderboard.First(u => u.player.ID == currentPlayer.ID).AverageGoalsAgainstPerGame)
                .Count() + 1;
            return betterStat;
        }

        public IEnumerable<DisplayGame> GamesPlayedIn(Guid playerID)
        {
            games = gameRetriever.GetAllGames();
            return games
                .Where(r => r.BlueDefense == playerID || r.BlueOffense == playerID || r.RedDefense == playerID || r.RedOffense == playerID);
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
            players = playerRetriever.GetPlayers();
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
                    if (game.BlueScore >game.RedScore) { teammateStats[game.BlueOffense][1] += 1; }
                }
                else if (playerID == game.BlueOffense)
                {
                    if (game.BlueDefense == null) { continue; }
                    teammateStats[game.BlueDefense][0] += 1;
                    if (game.BlueScore>game.RedScore) { teammateStats[game.BlueDefense][1] += 1; }
                }
                else if (playerID == game.RedDefense)
                {
                    if (game.RedOffense == null) { continue; }
                    teammateStats[game.RedOffense][0] += 1;
                    if (game.RedScore >game.BlueScore) { teammateStats[game.RedOffense][1] += 1; }
                }
                else if (playerID == game.RedOffense)
                {
                    if (game.RedDefense == null) { continue; }
                    teammateStats[game.RedDefense][0] += 1;
                    if (game.RedScore >game.BlueScore) { teammateStats[game.RedDefense][1] += 1; }
                }
            }
            teammateStats.Remove(playerID);
        }

        public int[] AverageEloByPosition(Player player)
        {
            var playerData = leaderboard.FirstOrDefault(p => p.player.ID == player.ID);
            return new int[] { playerData.AverageOffenseElo, playerData.AverageDefenseElo };
        }
    }
}
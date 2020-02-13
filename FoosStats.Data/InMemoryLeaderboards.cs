using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FoosStats.Core
{
    public class InMemoryLeaderboards : ILeaderboards
    {
        private readonly IEnumerable<Player> players;
        private readonly ITeamRepository teamRepository;
        private IEnumerable<DisplayGame> games;
        public IEnumerable<DerivedPlayerData> leaderboard;
        public InMemoryLeaderboards(IPlayerRepository playerRepository, IGameRepository gameRepository, ITeamRepository teamRepository)
        {
            players = playerRepository.GetPlayers();
            games = gameRepository.GetGames();
            this.teamRepository = teamRepository;
            leaderboard = MakeLeaderboard();

        }
        public IEnumerable<DerivedPlayerData> GetFullLeaderboard()
        {
            return leaderboard;
        }

        public DerivedPlayerData GetDataById(Guid Id)
        {
            return leaderboard.FirstOrDefault(p => p.player.ID == Id);
        }

        public PlayerStandingData GetPlayerStandings(Guid Id)
        {
            return new PlayerStandingData
            {
                WinPercentStanding = leaderboard
                .Where(r => r.WinPercentage >
                leaderboard.First(u => u.player.ID == Id).WinPercentage)
                .Count() + 1,

                GoalsForAverageStanding = leaderboard
                .Where(r => r.AverageGoalsPerGame >
                leaderboard.First(u => u.player.ID == Id).AverageGoalsPerGame)
                .Count() + 1,

                GoalAgainstAverageStanding = leaderboard
                .Where(r => r.AverageGoalsAgainstPerGame <
                leaderboard.First(u => u.player.ID == Id).AverageGoalsAgainstPerGame)
                .Count() + 1,

                OffenceWinPercentStanding = leaderboard
                .Where(r => r.OffenceWinPct >
                leaderboard.First(u => u.player.ID == Id).OffenceWinPct)
                .Count() + 1,

                DefenseWinPercentStanding = leaderboard
                .Where(r => r.DefenseWinPct >
                leaderboard.First(u => u.player.ID == Id).DefenseWinPct)
                .Count() + 1,

                RedWinPercentStanding = leaderboard
                .Where(r => r.RedWinPct >
                leaderboard.First(u => u.player.ID == Id).RedWinPct)
                .Count() + 1,

                BlueWinPercentStanding = leaderboard
                .Where(r => r.BlueWinPct >
                leaderboard.First(u => u.player.ID == Id).BlueWinPct)
                .Count() + 1,

                AverageOffenseEloStanding = leaderboard
                .Where(r => r.AverageOffenseElo >
                leaderboard.First(u => u.player.ID == Id).AverageOffenseElo)
                .Count() + 1,

                AverageDefenseEloStanding = leaderboard
                .Where(r => r.AverageDefenseElo >
                leaderboard.First(u => u.player.ID == Id).AverageDefenseElo)
                .Count() + 1,
            };
        }

        public IEnumerable<DerivedPlayerData> GetLimitedLeaderboard()
        {
            return leaderboard.Where(p=>p.player.GamesPlayed >3);
        }
        public IEnumerable<DerivedPlayerData> MakeLeaderboard()
        {
            var lst =  new List<DerivedPlayerData>();
            foreach(var currentPlayer in players)
            {
                var avgElo = AverageEloByPosition(currentPlayer);
                lst.Add(new DerivedPlayerData
                {
                    player = currentPlayer,
                    WinPercentage= (float)currentPlayer.GamesWon/currentPlayer.GamesPlayed*100,
                    AverageGoalsPerGame = (float)currentPlayer.GoalsFor/currentPlayer.GamesPlayed,
                    AverageGoalsAgainstPerGame = (float)currentPlayer.GoalsAgainst/currentPlayer.GamesPlayed,
                    OffenceWinPct= OffenseWinPct(currentPlayer),
                    DefenseWinPct = DefenseWinPct(currentPlayer),
                    BlueWinPct = BlueSideWinPct(currentPlayer),
                    RedWinPct = RedSideWinPct(currentPlayer),
                    AverageOffenseElo= avgElo[0],
                    AverageDefenseElo = avgElo[1]
                });
            }
            return lst;
        }
        public void Update()
        {
            leaderboard = MakeLeaderboard();
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
        public int[] AverageEloByPosition(Player currentPlayer)
        {
            var offenseTeams = teamRepository.GetTeams().Where(t => t.OffenseID == currentPlayer.ID);
            var defenseTeams = teamRepository.GetTeams().Where(t => t.DefenseID == currentPlayer.ID);

            var offenseNum = 0;
            var offenseDenom = 0;
            var defenseNum = 0;
            var defenseDenom = 0;

            foreach (var team in offenseTeams)
            {
                offenseNum += team.Rank * team.GamesPlayed;
                offenseDenom += team.GamesPlayed;
            }
            foreach (var team in defenseTeams)
            {
                defenseNum += team.Rank * team.GamesPlayed;
                defenseDenom += team.GamesPlayed;
            }
            var offenseAvg = (int)Math.Round((float)offenseNum / offenseDenom);
            var defenseAvg = (int)Math.Round((float)defenseNum / defenseDenom);
            if (offenseAvg < 0) { offenseAvg = 0; }
            if (defenseAvg < 0) { defenseAvg = 0; }
            return new int[] { offenseAvg, defenseAvg };
        }
    }

}
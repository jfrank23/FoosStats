using System;
using System.Collections.Generic;
using System.Linq;
using FoosStats.Core;
using FoosStats.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoosStats2.Controllers.Leaderboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboards leaderboard;

        public LeaderboardController(ILeaderboards leaderboard)
        {
            this.leaderboard = leaderboard;
        }

        [HttpGet]
        public IEnumerable<DerivedPlayerData> GetLimitedLeaderboard()
        {
            return leaderboard.GetLimitedLeaderboard();
        }

        [HttpGet]
        [Route("Full")]
        public IEnumerable<DerivedPlayerData> GetLeaderboard()
        {
            return leaderboard.GetFullLeaderboard();
        }

        [HttpGet]
        [Route("{stat}/{top}")]
        [Route("{stat}")]
        public IEnumerable<DerivedPlayerData> GetTopPlayers(string stat,int top = 0)
        {
            var ordered = DetermineStat(stat);
            if (top == 0)
            {
                return ordered;
            }
            return ordered.Take(top);
        }

        private IEnumerable<DerivedPlayerData> DetermineStat(string stat)
        {
            var ordered = Enumerable.Empty<DerivedPlayerData>();

            switch (stat.ToLower())
            {
                case "winpercentage":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.WinPercentage);
                    break;
                case "goalsforaverage":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.AverageGoalsPerGame);
                    break;
                case "goalsagainstaverage":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderBy(r => r.AverageGoalsAgainstPerGame);
                    break;
                case "winpercentagered":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.RedWinPct);
                    break;
                case "winpercentageblue":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.BlueWinPct);
                    break;
                case "winpercentageoffense":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.OffenceWinPct);
                    break;
                case "winpercentagedefense":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.DefenseWinPct);
                    break;
                case "offenseelo":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.AverageOffenseElo);
                    break;
                case "defenseelo":
                    ordered = leaderboard.GetLimitedLeaderboard().OrderByDescending(r => r.AverageDefenseElo);
                    break;
            }

            return ordered;
        }
    }
}
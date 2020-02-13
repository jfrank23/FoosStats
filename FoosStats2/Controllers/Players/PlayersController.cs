using System;
using System.Collections.Generic;
using FoosStats.Core;
using FoosStats.Core.Repositories;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;

namespace FoosStats2.Controllers.Players
{
    [Route("api")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRetriever playerRetriver;
        private readonly ILeaderboards leaderboard;

        public PlayersController(IPlayerRetriever playerRetriver, ILeaderboards leaderboard)
        {
            this.playerRetriver = playerRetriver;
            this.leaderboard = leaderboard;
        }

        [Route("Players")]
        [HttpGet]
        public IEnumerable<Player> GetAllPlayers()
        {
            var players = playerRetriver.GetPlayers();
            return playerRetriver.GetPlayers();
        }

        [HttpGet]
        [Route("player/{id}")]
        public Player GetPlayerById(Guid id)
        {
            return playerRetriver.GetPlayerById(id);
        }

        [HttpGet]
        [Route("player/AdvancedStats/{id}")]
        public DerivedPlayerData GetAdvancedStatsById(Guid id)
        {
            return leaderboard.GetDataById(id);
        }

        [HttpGet]
        [Route("player/Standings/{id}")]
        public PlayerStandingData GetStandingsById(Guid id)
        {
            return leaderboard.GetPlayerStandings(id);
        }
    }
}
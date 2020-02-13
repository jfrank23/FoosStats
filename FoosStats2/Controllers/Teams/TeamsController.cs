using System;
using System.Collections.Generic;
using System.Linq;
using FoosStats.Core;
using FoosStats.Core.Repositories;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;

namespace FoosStats2.Controllers.Teams
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRetriever teamRetriever;
        private readonly IHistoricalData historicalData;
        private readonly IGameRetriever gameRetriever;

        public TeamsController(ITeamRetriever teamRetriever, IHistoricalData historicalData, IGameRetriever gameRetriever)
        {
            this.teamRetriever = teamRetriever;
            this.historicalData = historicalData;
            this.gameRetriever = gameRetriever;
        }

        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            return teamRetriever.GetAllTeams();
        }

        [HttpGet]
        [Route("BestByPosition")]
        public IEnumerable<Team> GetBestTeamsByPosition()
        {
            return teamRetriever.BestTeamsByPosition();
        }

        [HttpGet]
        [Route("BestOverall")]
        public IEnumerable<Team> GetBestOverall()
        {
            return teamRetriever.BestOverallTeams();
        }

        [HttpGet]
        [Route("{id}")]
        public Team GetTeamById(Guid id)
        {
            return teamRetriever.GetTeamById(id);
        }

        [HttpGet]
        [Route("{defenseId}/{offenseId}")]
        public Team GetBestTeamsByPosition(Guid defenseId, Guid offenseId)
        {
            return teamRetriever.GetTeamByPlayers(defenseId,offenseId);
        }

        [HttpGet]
        [Route("historical/{teamId}")]
        public List<int> GetHistoricalData(Guid teamId)
        {
            var team = teamRetriever.GetTeamById(teamId);
            return historicalData.GetHistoricalTeamRanks()[$"{team.DefenseID} {team.OffenseID}"];
        }

        [HttpGet]
        [Route("gamesInvolved/{teamId}")]
        public IEnumerable<DisplayGame> GetGamesInvolved(Guid teamId)
        {
            var team = teamRetriever.GetTeamById(teamId);
            return gameRetriever.GetAllGames().Where(g => (g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID) || (g.RedDefense == team.DefenseID && g.RedOffense == team.OffenseID));
        }
    }
}
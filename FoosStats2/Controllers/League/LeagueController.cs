using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoosStats2.Controllers.League
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IGameRetriever gameRetriever;

        public LeagueController(IGameRetriever gameRetriever)
        {
            this.gameRetriever = gameRetriever;
        }

        [HttpGet]
        [Route("winPerSide")]
        public IEnumerable<int> GetSideBreakdown()
        {
            var blueWins = gameRetriever.GetAllGames().Where(g => g.BlueScore > g.RedScore).Count();
            var redWins = gameRetriever.GetAllGames().Where(g => g.BlueScore < g.RedScore).Count();
            return new[] { blueWins, redWins };
        }

        [HttpGet]
        [Route("GamesPlayed")]
        public int GetGamesPlayed()
        {
            return gameRetriever.GetAllGames().Count();
        }
    }
}
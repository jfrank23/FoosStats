using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoosStats2.Controllers.Games
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRetriever gameRetriever;

        public GamesController(IGameRetriever gameRetriever)
        {
            this.gameRetriever = gameRetriever;
        }

        [HttpGet]
        public IEnumerable<DisplayGame> GetAllGames()
        {
            return gameRetriever.GetAllGames();
        }

        [HttpGet]
        [Route("{id}")]
        public Game GetGameById(Guid id)
        {
            return gameRetriever.GetGameById(id);
        }
    }
}
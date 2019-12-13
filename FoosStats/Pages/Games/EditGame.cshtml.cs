using System;
using System.Collections.Generic;
using FoosStats.Core;
using FoosStats.Core.Creators;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace FoosStats.Pages.Games
{
    public class EditGameModel : PageModel
    {
        public IEnumerable<Game> games;
        public IEnumerable<Player> players;
        public IGameRetriever gameRetriever;
        public IPlayerRetriever playerRetriever;
        public IUpdater<Game> gameUpdater;
        private readonly ITeamUpdater teamUpdater;
        private readonly ITeamRefresher teamRefresher;
        private readonly IWebHostEnvironment env;
        public ICreator<Game> gameCreator;

        [BindProperty]
        public Game game { get; set; }
        public EditGameModel(IGameRetriever gameRetriever, 
            IPlayerRetriever playerRetriever, ICreator<Game> gameCreator, 
            IUpdater<Game> gameUpdater, ITeamUpdater teamUpdater, 
            ITeamRefresher teamRefresher, IWebHostEnvironment env)
        {
            this.gameRetriever = gameRetriever;
            this.playerRetriever = playerRetriever;
            this.gameCreator = gameCreator;
            this.gameUpdater = gameUpdater;
            this.teamUpdater = teamUpdater;
            this.teamRefresher = teamRefresher;
            this.env = env;
        }
        public IActionResult OnGet(Guid gameID)
        {
            if (!env.IsDevelopment())
            {
                return Redirect("../NoPermission");
            }
            games = gameRetriever.GetAllGames();
            players = playerRetriever.GetPlayers();
            game = gameRetriever.GetGameById(gameID);
            return Page();

        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Guid.Equals(game.GameID, Guid.Empty))
            {
                gameUpdater.Update(game);
                teamRefresher.Refresh();
            }
            else
            {
                gameCreator.Create(game);
                teamUpdater.Update(game);

            }
            TempData["Message"] = "Game saved!";
            return RedirectToPage("./List");
        }
    }
}
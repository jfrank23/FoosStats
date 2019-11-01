using System;
using System.Collections.Generic;
using FoosStats.Core;
using FoosStats.Core.Creators;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Games
{
    public class EditGameModel : PageModel
    {
        public IEnumerable<Game> games;
        public IEnumerable<Player> players;
        public IGameRetriever gameRetriever;
        public IPlayerRetriever playerRetriever;
        public IUpdater<Game> gameUpdater;
        public ICreator<Game> gameCreator;

        [BindProperty]
        public Game game { get; set; }
        public EditGameModel(IGameRetriever gameRetriever, IPlayerRetriever playerRetriever, ICreator<Game> gameCreator, IUpdater<Game> gameUpdater)
        {
            this.gameRetriever = gameRetriever;
            this.playerRetriever = playerRetriever;
            this.gameCreator = gameCreator;
            this.gameUpdater = gameUpdater;
        }
        public void OnGet(Guid gameID)
        {
            games = gameRetriever.GetAllGames();
            players = playerRetriever.GetPlayersByName();
            game = gameRetriever.GetGameById(gameID);
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
            }
            else
            {
                gameCreator.Create(game);
            }
            TempData["Message"] = "Game saved!";
            return RedirectToPage("./List");
        }
    }
}
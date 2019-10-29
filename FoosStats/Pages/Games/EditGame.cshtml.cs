using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core;
using FoosStats.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Games
{
    public class EditGameModel : PageModel
    {
        public IEnumerable<Game> games;
        public IGameRepository gameRepo;
        public IPlayerRepository playerRepo;
        public IEnumerable<Player> players;

        [BindProperty]
        public Game game { get; set; }
        public EditGameModel(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            this.gameRepo = gameRepository;
            this.playerRepo = playerRepository;
        }
        public void OnGet(Guid gameID)
        {
            games = gameRepo.GetGames();
            players = playerRepo.GetPlayersByName();
            game = gameRepo.GetGameByID(gameID);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Guid.Equals(game.GameID, Guid.Empty))
            {
                gameRepo.Update(game);
            }
            else
            {
                gameRepo.Add(game);
            }
            TempData["Message"] = "Game saved!";
            return RedirectToPage("./List");
        }
    }
}
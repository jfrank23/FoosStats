using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoosStats.Core;

namespace FoosStats.Pages.Games
{
    public class ListModel : PageModel
    {
        public IGameRepository gameRepo;
        public IPlayerRepository playerRepo;
        public IEnumerable<DisplayGame> games;

        public ListModel(IGameRepository gameRepository, IPlayerRepository playerRepo)
        {
            this.gameRepo = gameRepository;
            this.playerRepo = playerRepo;
        }
        public void OnGet()
        {
            games = gameRepo.GetGames();
        }
    }
}
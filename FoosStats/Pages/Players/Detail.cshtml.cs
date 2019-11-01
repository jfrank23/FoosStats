using System;
using System.Collections.Generic;
using FoosStats.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace FoosStats.Pages.Players
{
    public class DetailModel : PageModel
    {
        public IPlayerRepository playerRepo;
        private IGameRepository gameRepo;
        public IEnumerable<DisplayGame> top3Games { get; set; }
        public Player Player { get; private set; }
        
        

        public DetailModel(IPlayerRepository playerRepository, IGameRepository gameRepository)
        {
            this.playerRepo = playerRepository;
            this.gameRepo = gameRepository;
        }

        public IActionResult OnGet(Guid playerID)
        {
            Player = playerRepo.GetPlayerById(playerID);
            if (Player== null)
            {
                return RedirectToPage("./NotFound");
            }
            top3Games = gameRepo.GetGames()
                .Where(r => r.BlueDefense==playerID || r.BlueOffense==playerID || r.RedDefense==playerID ||r.RedOffense==playerID)
                .ToList()
                .Take(3);
            return Page();
        }
    }
}
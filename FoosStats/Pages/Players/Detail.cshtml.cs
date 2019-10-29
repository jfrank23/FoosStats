using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core;
using FoosStats.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Players
{
    public class DetailModel : PageModel
    {
        private IPlayerRepository playerRepo;
        public Player Player { get; private set; }

        public DetailModel(IPlayerRepository playerRepository)
        {
            this.playerRepo = playerRepository;
        }

        public IActionResult OnGet(Guid playerID)
        {
            Player = playerRepo.GetPlayerById(playerID);
            if (Player== null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}
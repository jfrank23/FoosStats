using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoosStats.Data;
using FoosStats.Core;

namespace FoosStats.Pages.Players
{
    public class EditPlayerModel : PageModel
    {
        private readonly IPlayerRepository playerRepo;
        [BindProperty]
        public Player Player { get; set; }

        public EditPlayerModel(IPlayerRepository playerRepository)
        {
            this.playerRepo = playerRepository;
        }

        public IActionResult OnGet(Guid? playerID)
        {
            if (playerID.HasValue)
            {
                Player = playerRepo.GetPlayerById(playerID.Value);
            }
            else
            {
                Player = new Player();
            }

            if(Player == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if(!Guid.Equals(Player.ID,Guid.Empty))
            {
                playerRepo.Update(Player);
            }
            else
            {
                playerRepo.Add(Player);
            }
            TempData["Message"] = "Player saved!";
            return RedirectToPage("./List");
        }
    }
}
using System;
using FoosStats.Core;
using FoosStats.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Players
{
    public class DeletePlayerModel : PageModel
    {
        public IPlayerRepository playerRepo;
        public Player player;
        public DeletePlayerModel(IPlayerRepository playerRepository)
        {
            this.playerRepo = playerRepository;
        }

        public IActionResult OnGet(Guid playerID)
        {
            if (playerID == Guid.Empty)
            {
                return Redirect("./NotFound");
            }
            player = playerRepo.GetPlayerById(playerID);
            return Page();

        }
        public IActionResult OnPost(Guid playerID)
        {
            playerRepo.Delete(playerID);
            return Redirect("~/Players/List");
        }
    }
}

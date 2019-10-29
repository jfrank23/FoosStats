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
    public class DeleteGameModel : PageModel
    {
        public IGameRepository gameRepo;
        public DeleteGameModel(IGameRepository gameRepository)
        {
            this.gameRepo = gameRepository;
        }

        public IActionResult OnGet(Guid gameID)
        {
            if (gameID == Guid.Empty)
            {
                return Redirect("./NotFound");
            }
            return Page();
            
        }
        public IActionResult OnPost(Guid gameID)
        {
            gameRepo.Delete(gameID);
            return Redirect("~/Games/List");
        }
    }
}
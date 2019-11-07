using System;
using FoosStats.Core;
using FoosStats.Core.Deleters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Games
{
    public class DeleteGameModel : PageModel
    {
        public IDeleter<Game> gameDeleter;

        public DeleteGameModel(IDeleter<Game> gameDeleter)
        {
            this.gameDeleter = gameDeleter;
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
            gameDeleter.Delete(gameID);
            return Redirect("~/Games/List");
        }
    }
}
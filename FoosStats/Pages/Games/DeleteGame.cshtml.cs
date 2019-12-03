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
        private readonly ITeamRefresher teamRefresher;

        public DeleteGameModel(IDeleter<Game> gameDeleter, ITeamRefresher teamRefresher)
        {
            this.gameDeleter = gameDeleter;
            this.teamRefresher = teamRefresher;
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
            teamRefresher.Refresh();
            return Redirect("~/Games/List");
        }
    }
}
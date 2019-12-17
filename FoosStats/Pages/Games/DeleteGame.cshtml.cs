using System;
using FoosStats.Core;
using FoosStats.Core.Deleters;
using FoosStats.Core.Updaters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Games
{
    public class DeleteGameModel : PageModel
    {
        public IDeleter<Game> gameDeleter;
        private readonly ITeamUpdater teamUpdater;

        public DeleteGameModel(IDeleter<Game> gameDeleter, ITeamUpdater teamUpdater)
        {
            this.gameDeleter = gameDeleter;
            this.teamUpdater = teamUpdater;
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
            teamUpdater.Refresh();
            return Redirect("~/Games/List");
        }
    }
}
using System;
using FoosStats.Core;
using FoosStats.Core.Deleters;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace FoosStats.Pages.Players
{
    public class DeletePlayerModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        private readonly ITeamUpdater teamUpdater;
        private readonly IWebHostEnvironment env;
        public IDeleter<Player> playerDeleter;
        public Player player;
        public DeletePlayerModel(IPlayerRetriever playerRetriever, ITeamUpdater teamUpdater, IWebHostEnvironment env)
        {
            this.playerRetriever = playerRetriever;
            this.teamUpdater = teamUpdater;
            this.env = env;
        }

        public IActionResult OnGet(Guid playerID)
        {
            if (!env.IsDevelopment())
            {
                return Redirect("~/NoPermission");
            }
            if (playerID == Guid.Empty)
            {
                return Redirect("./NotFound");
            }
            player = playerRetriever.GetPlayerById(playerID);
            return Page();

        }
        public IActionResult OnPost(Guid playerID)
        {
            playerDeleter.Delete(playerID);
            teamUpdater.Refresh();
            return Redirect("~/Players/List");
        }
    }
}

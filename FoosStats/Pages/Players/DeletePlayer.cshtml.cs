using System;
using FoosStats.Core;
using FoosStats.Core.Deleters;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Players
{
    public class DeletePlayerModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        public IDeleter<Player> playerDeleter;
        public Player player;
        public DeletePlayerModel(IPlayerRetriever playerRetriever, IDeleter<Player> playerDeleter)
        {
            this.playerRetriever = playerRetriever;
            this.playerDeleter = playerDeleter;
        }

        public IActionResult OnGet(Guid playerID)
        {
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
            return Redirect("~/Players/List");
        }
    }
}

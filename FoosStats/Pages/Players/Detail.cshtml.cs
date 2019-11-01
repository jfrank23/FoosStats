using System;
using System.Collections.Generic;
using FoosStats.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using FoosStats.Core.Retrievers;

namespace FoosStats.Pages.Players
{
    public class DetailModel : PageModel
    {
        public IPlayerRetriever playerRetriever;
        public IGameRetriever gameRetriever;
        public IEnumerable<DisplayGame> top3Games { get; set; }
        public Player Player { get; private set; }



        public DetailModel(IPlayerRetriever playerRetriever, IGameRetriever gameRetriever)
        {
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
        }

        public IActionResult OnGet(Guid playerID)
        {
            Player = playerRetriever.GetPlayerById(playerID);
            if (Player== null)
            {
                return RedirectToPage("./NotFound");
            }
            top3Games = gameRetriever.GetAllGames()
                .Where(r => r.BlueDefense==playerID || r.BlueOffense==playerID || r.RedDefense==playerID || r.RedOffense==playerID)
                .ToList()
                .Take(3);
            return Page();
        }
    }
}
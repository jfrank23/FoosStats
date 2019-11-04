using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoosStats.Core;
using FoosStats.Core.Retrievers;

namespace FoosStats.Pages.Games
{
    public class ListModel : PageModel
    {
        private IGameRetriever gameRetriever;
        public IEnumerable<DisplayGame> games;

        public ListModel(IGameRetriever gameRetriever)
        {
            this.gameRetriever = gameRetriever;
        }
        public void OnGet()
        {
            games = gameRetriever.GetAllGames();
        }
    }
}
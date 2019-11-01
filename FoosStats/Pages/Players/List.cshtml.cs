using System.Collections.Generic;
using FoosStats.Core;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoosStats.Pages.Players
{
    public class ListModel : PageModel
    {
        public IPlayerRetriever playerRetreiver;
        public IEnumerable<Player> players;
        public IConfiguration config;

        [BindProperty (SupportsGet = true)]
        public string SearchTerm { get; set; }
        public ListModel(IPlayerRetriever playerRetreiver)
        {
            this.playerRetreiver = playerRetreiver;
        }
        public void OnGet()
        {
            players = playerRetreiver.GetPlayersByName(SearchTerm);
        }
    }
}
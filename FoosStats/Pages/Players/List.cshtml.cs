using System.Collections.Generic;
using FoosStats.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoosStats.Pages.Players
{
    public class ListModel : PageModel
    {
        public IPlayerRepository PlayerRepo;
        public IEnumerable<Player> players;
        public IConfiguration config;

        [BindProperty (SupportsGet = true)]
        public string SearchTerm { get; set; }
        public ListModel(IPlayerRepository playerRepository, IConfiguration config)
        {
            this.PlayerRepo = playerRepository;
            this.config = config;
        }
        public void OnGet()
        {
            players = PlayerRepo.GetPlayersByName(SearchTerm);
        }
    }
}
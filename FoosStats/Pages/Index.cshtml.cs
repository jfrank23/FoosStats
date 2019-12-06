using FoosStats.Core;
using FoosStats.Core.PageSpecific;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FoosStats.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IHomePageStatRetriever homePageStatRetriever;
        public readonly ITeamRetriever teamRetriever;
        public DerivedPlayerData bestOnBlue;
        public DerivedPlayerData bestOnRed;

        public float RedWinPct { get; set; }
        public float BlueWinPct { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IHomePageStatRetriever homePageStatRetriever, ITeamRetriever teamRetriever)
        {
            _logger = logger;
            this.homePageStatRetriever = homePageStatRetriever;
            this.teamRetriever = teamRetriever;
        }

        public IActionResult OnGet()
        {
            var gamesPlayed = homePageStatRetriever.GamesPlayed();
            if (gamesPlayed > 3)
            {
                var sideWinArr = homePageStatRetriever.RedVsBlue();
                RedWinPct = (float)sideWinArr[0] / (sideWinArr[0] + sideWinArr[1]);
                BlueWinPct = (float)sideWinArr[1] / (sideWinArr[0] + sideWinArr[1]);
                bestOnRed = homePageStatRetriever.TopPlayersOnRed().FirstOrDefault();
                bestOnBlue = homePageStatRetriever.TopPlayersOnBlue().FirstOrDefault();
                return Page();

            }
            else
            {
                return RedirectToPage("./NoData");
            }
        }
    }
}

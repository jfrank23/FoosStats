using FoosStats.Core;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FoosStats.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IHomePageStatRetriever homePageStatRetriever;
        public DerivedData bestOnBlue;
        public DerivedData bestOnRed;

        public float RedWinPct { get; set; }
        public float BlueWinPct { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IHomePageStatRetriever homePageStatRetriever)
        {
            _logger = logger;
            this.homePageStatRetriever = homePageStatRetriever;
        }

        public void OnGet()
        {
            var sideWinArr = homePageStatRetriever.RedVsBlue();
            RedWinPct = (float)sideWinArr[0] / (sideWinArr[0] + sideWinArr[1]);
            BlueWinPct = (float)sideWinArr[1] / (sideWinArr[0] + sideWinArr[1]);
            bestOnRed = homePageStatRetriever.BestOnRed();
            bestOnBlue = homePageStatRetriever.BestOnBlue();
        }
    }
}

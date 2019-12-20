using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core.PageSpecific;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages
{
    public class FireTVModel : PageModel
    {
        public readonly IHomePageStatRetriever homePageStatRetriever;

        public FireTVModel(IHomePageStatRetriever homePageStatRetriever)
        {
            this.homePageStatRetriever = homePageStatRetriever;
        }
        public void OnGet()
        {

        }
    }
}
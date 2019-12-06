using FoosStats.Core;
using FoosStats.Core.PageSpecific;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace FoosStats.Pages.Teams
{
    public class DetailModel : PageModel
    {
        private readonly TeamDetailRetriever teamDetailRetriever;
        public IEnumerable<DisplayGame> games;
        public List<int> historicalElo;
        public List<int> labels;

        public DetailModel(TeamDetailRetriever teamDetailRetriever)
        {
            this.teamDetailRetriever = teamDetailRetriever;
        }
        public void OnGet(Guid teamID)
        {
            labels = new List<int>();
            games = teamDetailRetriever.GetGamesInvolved(teamID);
            historicalElo = teamDetailRetriever.GetHistoricalEloForTeam(teamID);
            for(var i=0; i< historicalElo.Count; i++)
            {
                labels.Add(i);
            }
        }
    }
}
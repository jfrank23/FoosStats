using System.Collections.Generic;
using FoosStats.Core;
using FoosStats.Core.Retrievers;
using FoosStats.Core.Updaters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages.Teams
{
    public class List : PageModel
    {
        private readonly ITeamRetriever teamStatsRetriever;
        private readonly ITeamUpdater teamUpdater;
        public IEnumerable<DisplayTeam> teamsByPosition;
        public IEnumerable<DisplayTeam> overallTeams;

        public List(ITeamRetriever teamStatsRetriever, ITeamUpdater teamUpdater)
        {
            this.teamStatsRetriever = teamStatsRetriever;
            this.teamUpdater = teamUpdater;
        }
        public void OnGet()
        {
            teamsByPosition = teamStatsRetriever.BestTeamsByPosition();
            overallTeams = teamStatsRetriever.BestOverallTeams();
        }
        public IActionResult OnPost()
        {
            teamUpdater.Refresh();
            teamsByPosition = teamStatsRetriever.BestTeamsByPosition();
            overallTeams = teamStatsRetriever.BestOverallTeams();
            return Page();
        }
    }
}
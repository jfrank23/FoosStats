using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosStats.Core;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoosStats.Pages
{
    public class TeamStatsModel : PageModel
    {
        private readonly ITeamStatsRetriever teamStatsRetriever;
        private readonly ITeamRefresher teamRefresher;
        public IEnumerable<DisplayTeam> teamsByPosition;
        public IEnumerable<DisplayTeam> overallTeams;

        public TeamStatsModel(ITeamStatsRetriever teamStatsRetriever, ITeamRefresher teamRefresher)
        {
            this.teamStatsRetriever = teamStatsRetriever;
            this.teamRefresher = teamRefresher;
        }
        public void OnGet()
        {
            teamsByPosition = teamStatsRetriever.BestTeamsByPosition();
            overallTeams = teamStatsRetriever.BestOverallTeams();
        }
        public IActionResult OnPost()
        {
            teamRefresher.Refresh();
            teamsByPosition = teamStatsRetriever.BestTeamsByPosition();
            overallTeams = teamStatsRetriever.BestOverallTeams();
            return Page();
        }
    }
}
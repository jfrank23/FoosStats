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
        public IEnumerable<Team> teamsByPosition;
        public IEnumerable<Team> overallTeams;

        public TeamStatsModel(ITeamStatsRetriever teamStatsRetriever)
        {
            this.teamStatsRetriever = teamStatsRetriever;
        }
        public void OnGet()
        {
            teamsByPosition = teamStatsRetriever.BestTeamsByPosition();
            overallTeams = teamStatsRetriever.BestOverallTeams();
        }
    }
}
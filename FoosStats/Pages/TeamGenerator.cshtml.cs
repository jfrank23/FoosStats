using FoosStats.Core;
using FoosStats.Core.PageSpecific;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace FoosStats.Pages
{
    public class TeamGeneratorModel : PageModel
    {
        public ITeamGenerator teamGenerator;

        [BindProperty]
        public IEnumerable<Guid> selectedPlayers{ get; set; }
        public List<List<String>> teams = new List<List<string>>();
        public List<DisplayTeam> fairTeams = new List<DisplayTeam>();
        public List<DisplayTeam> fairTeamsBenched;

        public TeamGeneratorModel(ITeamGenerator teamGenerator)
        {
            this.teamGenerator = teamGenerator;
        }
        public void OnGet()
        {
            teams.Add(new List<string>());
            teams.Add(new List<string>());
            teams.Add(new List<string>());

        }
        public void OnPost()
        {
            teams = teamGenerator.RandomTeams(selectedPlayers);
            fairTeams = teamGenerator.FairTeams(selectedPlayers);
            fairTeamsBenched = fairTeams.GetRange(2, fairTeams.Count - 2);
        }
    }
}
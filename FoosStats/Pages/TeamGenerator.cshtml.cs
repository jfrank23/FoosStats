using FoosStats.Core;
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
            teams = teamGenerator.Randomize(selectedPlayers);
        }
    }
}
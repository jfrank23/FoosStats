using FoosStats.Core;
using FoosStats.Core.PageSpecific;
using FoosStats.Core.Retrievers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Pages.Teams
{
    public class DetailModel : PageModel
    {
        private readonly TeamDetailRetriever teamDetailRetriever;
        private readonly ITeamRetriever teamRetriever;
        private int bluePlayed;
        private int redPlayed;
        private int redWins;
        private int blueWins;
        public IEnumerable<DisplayGame> games;
        public int gamesPlayed;
        private int gamesWon;
        public int[] record;
        public int[] blueRecord;
        public int[] redRecord;
        public float redPct;
        public float bluePct;
        public float avgGoalsFor;
        public float avgGoalsAgainst;
        public List<int> historicalElo;
        public List<int> labels;
        public DisplayTeam team;

        public DetailModel(TeamDetailRetriever teamDetailRetriever, ITeamRetriever teamRetriever)
        {
            this.teamDetailRetriever = teamDetailRetriever;
            this.teamRetriever = teamRetriever;
        }
        public void OnGet(Guid teamID)
        {
            team = teamRetriever.GetTeamById(teamID);
            labels = new List<int>();
            games = teamDetailRetriever.GetGamesInvolved(teamID);

            gamesPlayed = games.Count();
            bluePlayed = games.Where(g => g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID).Count();
            redPlayed = games.Where(g => g.RedDefense == team.DefenseID && g.RedOffense == team.OffenseID).Count();
            redWins = games.Where(g => g.RedDefense == team.DefenseID && g.RedOffense == team.OffenseID && g.RedScore > g.BlueScore).Count();
            blueWins = games.Where(g => g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID && g.BlueScore > g.RedScore).Count();
            gamesWon = redWins + blueWins;

            record = new[] { gamesWon, gamesPlayed - gamesWon };
            redRecord = new int[] { redWins, redPlayed - redWins };
            blueRecord = new int[] { blueWins, bluePlayed - blueWins };
            redPct = (float) redWins / redPlayed *100;
            bluePct = (float) blueWins / bluePlayed * 100;

            var goalsFor = games.Where(g => g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID).Select(r => r.BlueScore).Sum() + games.Where(g => g.RedDefense == team.DefenseID && g.RedOffense == team.OffenseID).Select(r => r.RedScore).Sum();
            var goalsAgainst = games.Where(g => g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID).Select(r => r.RedScore).Sum() + games.Where(g => g.RedDefense == team.DefenseID && g.RedOffense == team.OffenseID).Select(r => r.BlueScore).Sum();
            avgGoalsFor = (float)goalsFor / gamesPlayed;
            avgGoalsAgainst = (float)goalsAgainst / gamesPlayed;

            historicalElo = teamDetailRetriever.GetHistoricalEloForTeam(teamID);
            for(var i=0; i< historicalElo.Count; i++)
            {
                labels.Add(i);
            }
        }
    }
}
﻿using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoosStats.Core.Retrievers
{
    public interface ITeamRetriever
    {
        IEnumerable<DisplayTeam> BestOverallTeams();
        IEnumerable<DisplayTeam> BestTeamsByPosition();
        IEnumerable<DisplayTeam> GetAllTeams();
        DisplayTeam GetTeamById(Guid teamID);
        DisplayTeam GetTeamByPlayers(Guid DefenseID, Guid OffenseID);
    }
    public class TeamRetriever : ITeamRetriever
    {
        private readonly ITeamRepository teamRepository;
        private readonly IEnumerable<DisplayTeam> teams;

        public TeamRetriever(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
            teams = teamRepository.GetTeams();
        }
        public IEnumerable<DisplayTeam> BestOverallTeams()
        {
            var overallTeams = new List<DisplayTeam>();
            foreach (var team1 in teams)
            {
                if(overallTeams.Where(t=>(t.DefenseID == team1.DefenseID && t.OffenseID == team1.OffenseID)||(t.DefenseID == team1.OffenseID && t.OffenseID == team1.DefenseID)).Count() > 0) { continue; }

                var tempTeam = new DisplayTeam
                {
                    DefenseID = team1.DefenseID,
                    OffenseID = team1.OffenseID,
                    GamesPlayed = team1.GamesPlayed,
                    GamesWon = team1.GamesWon,
                    DefenseName = team1.DefenseName,
                    OffenseName = team1.OffenseName,
                    TeamID = team1.TeamID
                };
                foreach (var team2 in teams)
                {
                    
                    if((team1.DefenseID == team2.OffenseID) && (team1.OffenseID == team2.DefenseID))
                    {
                        tempTeam.GamesPlayed += team2.GamesPlayed;
                        tempTeam.GamesWon += team2.GamesWon;
                    }
                }
                tempTeam.WinPct = (float)tempTeam.GamesWon / tempTeam.GamesPlayed *100;
                overallTeams.Add(tempTeam);
            }
            return overallTeams.Where(t=>t.GamesPlayed>3).OrderByDescending(t=>t.WinPct);
        }
        public IEnumerable<DisplayTeam> BestTeamsByPosition()
        {
            return teams.Where(t => t.GamesPlayed >= 3).OrderByDescending(t => t.Rank);
        }
        public IEnumerable<DisplayTeam> GetAllTeams()
        {
            return teams;
        }
        public DisplayTeam GetTeamByPlayers(Guid DefenseID, Guid OffenseID)
        {
            return teamRepository.GetTeamByPlayers(DefenseID, OffenseID);
        }
        public DisplayTeam GetTeamById(Guid teamID)
        {
            return teamRepository.GetTeamById(teamID);
        }



        //public IEnumerable<DisplayTeam> BestOverallTeams()
        //{
        //    var bestOverall = new List<DisplayTeam>();
        //    foreach (var player1 in players)
        //    {
        //        foreach (var player2 in players)
        //        {
        //            if (bestOverall.Where(t => (t.DefenseID == player1.ID && t.OffenseID == player2.ID) || (t.DefenseID == player2.ID && t.OffenseID == player1.ID)).Count() > 0)
        //            {
        //                continue;
        //            }
        //            var gamesBlue = games.Where(g => (g.BlueDefense == player1.ID && g.BlueOffense == player2.ID) || (g.BlueDefense == player2.ID && g.BlueOffense == player1.ID));
        //            var gamesRed = games.Where(g => (g.RedDefense == player1.ID && g.RedOffense == player2.ID) || (g.RedDefense == player2.ID && g.RedOffense == player1.ID));
        //            var gamesPlayed = gamesBlue.Count() + gamesRed.Count();
        //            var gamesWon = gamesBlue.Where(r => r.BlueScore > r.RedScore).Count() + gamesRed.Where(r => r.RedScore > r.BlueScore).Count();
        //            bestOverall.Add(new DisplayTeam
        //            {
        //                DefenseID = player1.ID,
        //                DefenseName = $"{player1.FirstName} {player1.LastName}",
        //                OffenseID = player2.ID,
        //                OffenseName = $"{player2.FirstName} {player2.LastName}",
        //                GamesPlayed = gamesPlayed,
        //                GamesWon = gamesWon,
        //                WinPct = (float)gamesWon / gamesPlayed * 100
        //            });
        //        }
        //    }
        //    return bestOverall.Where(t => t.GamesPlayed >= 3).OrderByDescending(t => ((float)t.GamesWon / t.GamesPlayed));
        //}
        //public IEnumerable<DisplayTeam> BestTeamsByPosition()
        //{
        //    foreach (var player1 in players)
        //    {
        //        foreach (var player2 in players)
        //        {
        //            var teamExists = teamsByPosition.Where(r => (r.DefenseID == player1.ID && r.OffenseID == player2.ID) || (r.OffenseID == player1.ID && r.DefenseID == player2.ID));
        //            if (teamExists.Count() == 2)
        //            {
        //                continue;
        //            }
        //            //Player 1 as offense
        //            var player1OffenseRed = games.Where(r => r.RedOffense == player1.ID && r.RedDefense == player2.ID);
        //            var player1OffenseBlue = games.Where(r => r.BlueOffense == player1.ID && r.BlueDefense == player2.ID);

        //            //Player 1 as defense
        //            var player1DefenseRed = games.Where(r => r.RedDefense == player1.ID && r.RedOffense == player2.ID);
        //            var player1DefenseBlue = games.Where(r => r.BlueDefense == player1.ID && r.BlueOffense == player2.ID);

        //            var gamesAsOffense = player1OffenseBlue.Count() + player1OffenseRed.Count();
        //            var winsAsOffense = player1OffenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1OffenseRed.Where(r => r.RedScore > r.BlueScore).Count();

        //            var gamesAsDefense = player1DefenseBlue.Count() + player1DefenseRed.Count();
        //            var winsAsDefense = player1DefenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1DefenseRed.Where(r => r.RedScore > r.BlueScore).Count();
        //            teamsByPosition.Add(
        //           new DisplayTeam
        //           {
        //               OffenseID = player1.ID,
        //               OffenseName = $"{player1.FirstName} {player1.LastName}",
        //               DefenseID = player2.ID,
        //               DefenseName = $"{player2.FirstName} {player2.LastName}",
        //               GamesPlayed = gamesAsOffense,
        //               GamesWon = winsAsOffense,
        //               WinPct = (float)winsAsOffense / gamesAsOffense * 100
        //           }); ;
        //            teamsByPosition.Add(
        //            new DisplayTeam
        //            {
        //                OffenseID = player2.ID,
        //                OffenseName = $"{player2.FirstName} {player2.LastName}",
        //                DefenseID = player1.ID,
        //                DefenseName = $"{player1.FirstName} {player1.LastName}",
        //                GamesPlayed = gamesAsDefense,
        //                GamesWon = winsAsDefense,
        //                WinPct = (float)winsAsDefense / gamesAsDefense * 100
        //            }); ;

        //        }
        //    }
        //    return teamsByPosition.Where(r => r.GamesPlayed >= 3).OrderByDescending(r => r.WinPct);
        //}
    }
}

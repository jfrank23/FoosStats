using FoosStats.Core.Retrievers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FoosStats.Core.Retrievers
{
    public interface ITeamStatsRetriever
    {
        IEnumerable<Team> BestOverallTeams();
        IEnumerable<Team> BestTeamsByPosition();
    }
    public class TeamStatsRetriever : ITeamStatsRetriever
    {
        private readonly IPlayerRetriever playerRetriever;
        private readonly IGameRetriever gameRetriever;
        private IEnumerable<Player> players;
        private IEnumerable<DisplayGame> games;
        public List<Team> teamsByPosition = new List<Team>();

        public TeamStatsRetriever(IPlayerRetriever playerRetriever, IGameRetriever gameRetriever)
        {
            this.playerRetriever = playerRetriever;
            this.gameRetriever = gameRetriever;
            players = playerRetriever.GetPlayersByName();
            games = gameRetriever.GetAllGames();
        }
        public IEnumerable<Team> BestOverallTeams()
        {
            var bestOverall = new List<Team>();
           foreach(var player1 in players)
            {
                foreach(var player2 in players)
                {
                    if (bestOverall.Where(t=>(t.Defense.ID==player1.ID && t.Offense.ID ==player2.ID)|| (t.Defense.ID == player2.ID && t.Offense.ID == player1.ID)).Count()>0)
                    {
                        continue;
                    }
                    var gamesBlue = games.Where(g => (g.BlueDefense == player1.ID && g.BlueOffense == player2.ID) || (g.BlueDefense == player2.ID && g.BlueOffense == player1.ID));
                    var gamesRed = games.Where(g => (g.RedDefense == player1.ID && g.RedOffense == player2.ID) || (g.RedDefense == player2.ID && g.RedOffense == player1.ID));
                    var gamesPlayed = gamesBlue.Count() + gamesRed.Count();
                    var gamesWon = gamesBlue.Where(r => r.BlueScore > r.RedScore).Count() + gamesRed.Where(r => r.RedScore > r.BlueScore).Count();
                    bestOverall.Add(new Team
                    {
                        Defense = player1,
                        Offense = player2,
                        GamesPlayed = gamesPlayed,
                        WinPct = (float)gamesWon / gamesPlayed *100
                    });
                }
            }
            return bestOverall.Where(t=>t.GamesPlayed >=3).OrderByDescending(t=>t.WinPct);
        }
        public IEnumerable<Team> BestTeamsByPosition()
        {
            foreach (var player1 in players)
            {
                foreach (var player2 in players)
                {
                    var teamExists = teamsByPosition.Where(r => (r.Defense.ID == player1.ID && r.Offense.ID == player2.ID) || (r.Offense.ID == player1.ID && r.Defense.ID == player2.ID));
                    if (teamExists.Count() ==2)
                    {
                        continue;
                    }
                    //Player 1 as offense
                    var player1OffenseRed = games.Where(r => r.RedOffense == player1.ID && r.RedDefense == player2.ID);
                    var player1OffenseBlue = games.Where(r => r.BlueOffense == player1.ID && r.BlueDefense == player2.ID);

                    //Player 1 as defense
                    var player1DefenseRed = games.Where(r => r.RedDefense == player1.ID && r.RedOffense == player2.ID);
                    var player1DefenseBlue = games.Where(r => r.BlueDefense == player1.ID && r.BlueOffense == player2.ID);

                    var gamesAsOffense = player1OffenseBlue.Count() + player1OffenseRed.Count();
                    var winsAsOffense = player1OffenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1OffenseRed.Where(r => r.RedScore > r.BlueScore).Count();

                    var gamesAsDefense = player1DefenseBlue.Count() + player1DefenseRed.Count();
                    var winsAsDefense = player1DefenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1DefenseRed.Where(r => r.RedScore > r.BlueScore).Count();

                        teamsByPosition.Add(
                       new Team
                       {
                           Offense = player1,
                           Defense = player2,
                           GamesPlayed = gamesAsOffense,
                           WinPct = (float)winsAsOffense / gamesAsOffense * 100
                       }); ;
                        teamsByPosition.Add(
                        new Team
                        {
                            Offense = player2,
                            Defense = player1,
                            GamesPlayed = gamesAsDefense,
                            WinPct = (float)winsAsDefense / gamesAsDefense * 100
                        }); ;
                    
                }
            }
            return teamsByPosition.Where(r=>r.GamesPlayed>=3).OrderByDescending(r=>r.WinPct);
        }
    }
}

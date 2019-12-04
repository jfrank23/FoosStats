using FoosStats.Core.Creators;
using FoosStats.Core.ELO;
using FoosStats.Core.Retrievers;
using System.Linq;

namespace FoosStats.Core.Updaters
{
    public interface ITeamUpdater
    {
        void Update(Game newGame);
    }
    public class TeamUpdater : ITeamUpdater
    {

        private readonly ITeamRepository teamRepository;
        private readonly IGameRetriever gameRetriever;
        private readonly ITeamStatsRetriever teamStatsRetriever;
        private readonly ICreator<Team> teamCreator;

        public TeamUpdater(ITeamRepository teamRepository, IGameRetriever gameRetriever, ITeamStatsRetriever teamStatsRetriever, ICreator<Team> teamCreator)
        {
            this.teamRepository = teamRepository;
            this.gameRetriever = gameRetriever;
            this.teamStatsRetriever = teamStatsRetriever;
            this.teamCreator = teamCreator;
        }
        public void Update(Game newGame)
        {
            var games = gameRetriever.GetAllGames();
            //win = {blue,red}
            var win = new[] { 0, 0 };
            if (newGame.BlueScore > newGame.RedScore)
            {
                win = new[] { 1, 0 };
            }
            else
            {
                win = new[] { 0, 1 };
            }

            var blueTeam = teamStatsRetriever.GetTeamByPlayers(newGame.BlueDefense,newGame.BlueOffense);
            var redTeam = teamStatsRetriever.GetTeamByPlayers(newGame.RedDefense, newGame.RedOffense);
            if (blueTeam == null)
            {
                teamCreator.Create(new Team
                {
                    DefenseID = newGame.BlueDefense,
                    OffenseID = newGame.BlueOffense,
                    GamesPlayed = 0,
                    GamesWon = 0,
                });
                blueTeam = new DisplayTeam
                {
                    DefenseID = newGame.BlueDefense,
                    OffenseID = newGame.BlueOffense,
                    GamesPlayed = 0,
                    GamesWon = 0,
                    Rank = EloHandler.StartingScore
                };

            }
            if (redTeam == null)
            {
                teamCreator.Create(new Team
                {
                    DefenseID = newGame.RedDefense,
                    OffenseID = newGame.RedOffense,
                    GamesPlayed = 0,
                    GamesWon = 0,
                });
                redTeam = new DisplayTeam
                {
                    DefenseID = newGame.RedDefense,
                    OffenseID = newGame.RedOffense,
                    GamesPlayed = 0,
                    GamesWon = 0,
                    Rank = EloHandler.StartingScore
                };
                
            }
            var updatedScores = EloHandler.UpdatedRanks(blueTeam, redTeam, newGame);
            blueTeam.GamesPlayed += 1;
            blueTeam.GamesWon += win[0];
            blueTeam.Rank = updatedScores[0];
            


            redTeam.GamesPlayed += 1;
            redTeam.GamesWon += win[1];
            redTeam.Rank = updatedScores[1];

            teamRepository.Update(blueTeam);
            teamRepository.Update(redTeam);
        }
    }

}

//var players = playerRetriever.GetPlayersByName();
//var games = gameRetriever.GetAllGames();
//var teamsByPosition = new List<Team>();

//foreach (var player1 in players)
//{
//    foreach (var player2 in players)
//    {
//        var teamExists = teamsByPosition.Where(r => (r.DefenseID == player1.ID && r.OffenseID == player2.ID) || (r.OffenseID == player1.ID && r.DefenseID == player2.ID));
//        if (teamExists.Count() == 2)
//        {
//            continue;
//        }
//        //Player 1 as offense
//        var player1OffenseRed = games.Where(r => r.RedOffense == player1.ID && r.RedDefense == player2.ID);
//        var player1OffenseBlue = games.Where(r => r.BlueOffense == player1.ID && r.BlueDefense == player2.ID);

//        //Player 1 as defense
//        var player1DefenseRed = games.Where(r => r.RedDefense == player1.ID && r.RedOffense == player2.ID);
//        var player1DefenseBlue = games.Where(r => r.BlueDefense == player1.ID && r.BlueOffense == player2.ID);

//        var gamesAsOffense = player1OffenseBlue.Count() + player1OffenseRed.Count();
//        var winsAsOffense = player1OffenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1OffenseRed.Where(r => r.RedScore > r.BlueScore).Count();

//        var gamesAsDefense = player1DefenseBlue.Count() + player1DefenseRed.Count();
//        var winsAsDefense = player1DefenseBlue.Where(r => r.BlueScore > r.RedScore).Count() + player1DefenseRed.Where(r => r.RedScore > r.BlueScore).Count();
//        teamRepository.Create(new Team
//        {
//            OffenseID = player1.ID,
//            DefenseID = player2.ID,
//            GamesPlayed = gamesAsOffense,
//            GamesWon = winsAsOffense,
//        });
//        teamRepository.Create(new Team
//        {
//            OffenseID = player2.ID,
//            DefenseID = player1.ID,
//            GamesPlayed = gamesAsDefense,
//            GamesWon = winsAsDefense,
//        });
//        teamsByPosition.Add(
//       new DisplayTeam
//       {
//           OffenseID = player1.ID,
//           OffenseName = $"{player1.FirstName} {player1.LastName}",
//           DefenseID = player2.ID,
//           DefenseName = $"{player2.FirstName} {player2.LastName}",
//           GamesPlayed = gamesAsOffense,
//           GamesWon = winsAsOffense,
//           WinPct = (float)winsAsOffense / gamesAsOffense * 100
//       }); ;
//        teamsByPosition.Add(
//        new DisplayTeam
//        {
//            OffenseID = player2.ID,
//            OffenseName = $"{player2.FirstName} {player2.LastName}",
//            DefenseID = player1.ID,
//            DefenseName = $"{player1.FirstName} {player1.LastName}",
//            GamesPlayed = gamesAsDefense,
//            GamesWon = winsAsDefense,
//            WinPct = (float)winsAsDefense / gamesAsDefense * 100
//        }); ;

//    }
//}
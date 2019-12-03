using FoosStats.Core;
using FoosStats.Core.ELO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace FoosStats.Data
{
    public class LiteTeamRepository : ITeamRepository
    {
        readonly string connectionString = "Data Source= " + "FoosData.db" + "; Version=3; BinaryGUID=False;";
        private readonly IGameRepository gameRepository;
        private readonly IPlayerRepository playerRepository;
        private IEnumerable<Player> players;

        public LiteTeamRepository(IGameRepository gameRepository, IPlayerRepository playerRepository, string connectionString = null)
        {
            if (connectionString != null)
            {
                this.connectionString = connectionString;
            }
            CheckDatabaseExists();
            this.gameRepository = gameRepository;
            this.playerRepository = playerRepository;
        }
        private void CheckDatabaseExists()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "Select * from Teams ";
                    var reader = command.ExecuteReader();
                    var teams = new List<DisplayTeam>();
                    while (reader.Read())
                    {
                        teams.Add(
                            new DisplayTeam
                            {
                                TeamID = new Guid(reader.GetString(0)),
                                DefenseID = new Guid(reader.GetString(1)),
                                OffenseID = new Guid(reader.GetString(2)),
                                GamesPlayed = reader.GetInt32(3),
                                GamesWon = reader.GetInt32(4),
                                Rank = reader.GetInt32(5)
                            });

                    }
                }
            }
            catch
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"CREATE TABLE \"Teams\" (\"TeamID\"    TEXT UNIQUE, \"DefenseID\" TEXT, \"OffenseID\" TEXT,  \"GamesPlayed\"   INTEGER, \"GamesWon\" INTEGER, \"Rank\"  INTEGER, PRIMARY KEY(\"TeamID\"))";
                    command.ExecuteNonQuery();
                }
                //TODO: Add functionality to retrospectively add teams and ranking if players already exist
            }
        }
        public Team Add(Team newTeam)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                newTeam.TeamID = Guid.NewGuid();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Insert into Teams(TeamID, DefenseID, OffenseID, GamesPlayed, GamesWon, Rank) values(@TeamID, @DefenseID, @OffenseID, @GamesPlayed, @GamesWon, @Rank)";
                command.Parameters.Add(new SQLiteParameter("@TeamID", newTeam.TeamID));
                command.Parameters.Add(new SQLiteParameter("@DefenseID", newTeam.DefenseID));
                command.Parameters.Add(new SQLiteParameter("@OffenseID", newTeam.OffenseID));
                command.Parameters.Add(new SQLiteParameter("@GamesPlayed", newTeam.GamesPlayed));
                command.Parameters.Add(new SQLiteParameter("@GamesWon", newTeam.GamesWon));
                command.Parameters.Add(new SQLiteParameter("@Rank", EloHandler.StartingScore));


                command.ExecuteNonQuery();

            }
            return newTeam;
        }

        public IEnumerable<DisplayTeam> GetTeams()
        {
            var teams = new List<DisplayTeam>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT " +
                    "TeamId, DefenseID, OffenseID, GamesPlayed, GamesWon, Rank, DFirst, DLast, OFirst, OLast " +
                    "From(SELECT * FROM Teams " +
                    "LEFT JOIN(SELECT PlayerId as DId,FirstName as DFirst,LastName as DLast from Players) ON DefenseID = DId " +
                    "LEFT JOIN(SELECT PlayerId as OId,FirstName as OFirst,LastName as OLast from Players) ON OffenseID = OId); ";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    teams.Add(new DisplayTeam
                    {
                        TeamID = new Guid(reader.GetString(0)),
                        DefenseID = new Guid(reader.GetString(1)),
                        OffenseID = new Guid(reader.GetString(2)),
                        GamesPlayed = reader.GetInt32(3),
                        GamesWon = reader.GetInt32(4),
                        Rank = reader.GetInt32(5),
                        DefenseName = $"{reader.GetString(6)} {reader.GetString(7)}",
                        OffenseName = $"{reader.GetString(8)} {reader.GetString(9)}",
                        WinPct = (float)reader.GetInt32(4) / reader.GetInt32(3) * 100

                    });
                }
            }
            return teams.OrderByDescending(r => (float)r.GamesWon / r.GamesPlayed);
        }
        public void Update(Game newGame)
        {
            var games = gameRepository.GetGames();
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
            //take in the new game, pull both teams (or add), and then apply the rank, winPct, and GamesPlayed changes
            var blueTeam = GetTeams().FirstOrDefault(g => (g.DefenseID == newGame.BlueDefense) && (g.OffenseID == newGame.BlueOffense));
            var redTeam = GetTeams().FirstOrDefault(g => (g.DefenseID == newGame.RedDefense) && (g.OffenseID == newGame.RedOffense));
            if (blueTeam == null)
            {
                blueTeam = Add(new DisplayTeam
                {
                    DefenseID = newGame.BlueDefense,
                    OffenseID = newGame.BlueOffense,
                    GamesPlayed = 1,
                    GamesWon = win[0],
                });
            }
            else
            {
                var blueTeamGamesPlayed = games.Where(g => (g.BlueDefense == blueTeam.DefenseID && g.BlueOffense == blueTeam.OffenseID) || (g.RedDefense == blueTeam.DefenseID && g.RedOffense == blueTeam.OffenseID)).Count();
                var blueTeamGamesWon = games.Where(g => (g.BlueDefense == blueTeam.DefenseID && g.BlueOffense == blueTeam.OffenseID && g.BlueScore > g.RedScore) || (g.RedDefense == blueTeam.DefenseID && g.RedOffense == blueTeam.OffenseID && g.RedScore > g.BlueScore)).Count();
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"Update Teams Set GamesPlayed = @GamesPlayed,GamesWon= @GamesWon,Rank= @Rank WHERE DefenseID = @DefenseID AND OffenseID = @OffenseID";
                    command.Parameters.Add(new SQLiteParameter("@GamesPlayed", blueTeamGamesPlayed));
                    command.Parameters.Add(new SQLiteParameter("@GamesWon", blueTeamGamesWon));
                    command.Parameters.Add(new SQLiteParameter("@Rank", EloHandler.UpdatedRanks(blueTeam, redTeam, newGame)[0]));
                    command.Parameters.Add(new SQLiteParameter("@DefenseID", blueTeam.DefenseID));
                    command.Parameters.Add(new SQLiteParameter("@OffenseID", blueTeam.OffenseID));

                    command.ExecuteNonQuery();
                }
            }

            if (redTeam == null)
            {
                redTeam = (DisplayTeam)Add(new Team
                {
                    DefenseID = newGame.RedDefense,
                    OffenseID = newGame.RedOffense,
                    GamesPlayed = 1,
                    GamesWon = win[1],
                });
            }
            else
            {
                var redTeamGamesPlayed = games.Where(g => (g.BlueDefense == redTeam.DefenseID && g.BlueOffense == redTeam.OffenseID) || (g.RedDefense == redTeam.DefenseID && g.RedOffense == redTeam.OffenseID)).Count();
                var redTeamGamesWon = games.Where(g => (g.BlueDefense == redTeam.DefenseID && g.BlueOffense == redTeam.OffenseID && g.BlueScore > g.RedScore) || (g.RedDefense == redTeam.DefenseID && g.RedOffense == redTeam.OffenseID && g.RedScore > g.BlueScore)).Count();
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"Update Teams Set GamesPlayed = @GamesPlayed,GamesWon= @GamesWon,Rank= @Rank WHERE DefenseID = @DefenseID AND OffenseID = @OffenseID";
                    command.Parameters.Add(new SQLiteParameter("@GamesPlayed", redTeamGamesPlayed));
                    command.Parameters.Add(new SQLiteParameter("@GamesWon", redTeamGamesWon));
                    command.Parameters.Add(new SQLiteParameter("@Rank", EloHandler.UpdatedRanks(blueTeam, redTeam, newGame)[1]));
                    command.Parameters.Add(new SQLiteParameter("@DefenseID", redTeam.DefenseID));
                    command.Parameters.Add(new SQLiteParameter("@OffenseID", redTeam.OffenseID));

                    command.ExecuteNonQuery();
                }
            }
        }
        //method that deletes all the teams a player is on if that player is deleted
        public void Delete(Guid playerID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete From Teams where OffenseID =@playerID or DefenseID = @playerID";
                command.Parameters.Add(new SQLiteParameter("@playerID", playerID));

                command.ExecuteNonQuery();
            }
        }
        public void Refresh()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete From Teams ";

                command.ExecuteNonQuery();
            }
            players = playerRepository.GetPlayersByName();
            var games = gameRepository.GetGames();
            var teamsByPosition = new List<Team>();

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
            //        Add(new Team
            //        {
            //            OffenseID = player1.ID,
            //            DefenseID = player2.ID,
            //            GamesPlayed = gamesAsOffense,
            //            GamesWon = winsAsOffense,
            //        });
            //        Add(new Team
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
            foreach (var game in games)
            {
                Update(game);
            }
        }
    }
}


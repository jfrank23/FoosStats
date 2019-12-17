using FoosStats.Core;
using FoosStats.Core.Repositories;
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

        public LiteTeamRepository(IGameRepository gameRepository, IPlayerRepository playerRepository, string connectionString = null)
        {
            if (connectionString != null)
            {
                this.connectionString = connectionString;
            }
            CheckTableExists();
            this.gameRepository = gameRepository;
            this.playerRepository = playerRepository;
        }
        private void CheckTableExists()
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
        public Team Create(Team newTeam)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                if(newTeam.TeamID == Guid.Empty)
                {
                    newTeam.TeamID = Guid.NewGuid();
                }
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Insert into Teams(TeamID, DefenseID, OffenseID, GamesPlayed, GamesWon, Rank) values(@TeamID, @DefenseID, @OffenseID, @GamesPlayed, @GamesWon, @Rank)";
                command.Parameters.Add(new SQLiteParameter("@TeamID", newTeam.TeamID));
                command.Parameters.Add(new SQLiteParameter("@DefenseID", newTeam.DefenseID));
                command.Parameters.Add(new SQLiteParameter("@OffenseID", newTeam.OffenseID));
                command.Parameters.Add(new SQLiteParameter("@GamesPlayed", newTeam.GamesPlayed));
                command.Parameters.Add(new SQLiteParameter("@GamesWon", newTeam.GamesWon));
                command.Parameters.Add(new SQLiteParameter("@Rank", newTeam.Rank));


                command.ExecuteNonQuery();

            }
            return newTeam;
        }
        public DisplayTeam GetTeamByPlayers(Guid DefenseID, Guid OffenseID)
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
                    "LEFT JOIN(SELECT PlayerId as OId,FirstName as OFirst,LastName as OLast from Players) ON OffenseID = OId) " +
                    "Where DefenseID = @DefenseID AND OffenseID = @OffenseID ;";
                command.Parameters.Add(new SQLiteParameter("@DefenseID", DefenseID));
                command.Parameters.Add(new SQLiteParameter("@OffenseID", OffenseID));

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
                return teams.FirstOrDefault();
            }
        }
        public DisplayTeam GetTeamById(Guid TeamID)
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
                    "LEFT JOIN(SELECT PlayerId as OId,FirstName as OFirst,LastName as OLast from Players) ON OffenseID = OId) " +
                    "Where TeamID = @TeamID ;";
                command.Parameters.Add(new SQLiteParameter("@TeamID", TeamID));
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
                return teams.FirstOrDefault();
            }
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
        public void Update(Team updatedTeam)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Update Teams Set GamesPlayed = @GamesPlayed,GamesWon= @GamesWon,Rank= @Rank WHERE DefenseID = @DefenseID AND OffenseID = @OffenseID";
                command.Parameters.Add(new SQLiteParameter("@GamesPlayed", updatedTeam.GamesPlayed));
                command.Parameters.Add(new SQLiteParameter("@GamesWon", updatedTeam.GamesWon));
                command.Parameters.Add(new SQLiteParameter("@Rank", updatedTeam.Rank));
                command.Parameters.Add(new SQLiteParameter("@DefenseID", updatedTeam.DefenseID));
                command.Parameters.Add(new SQLiteParameter("@OffenseID", updatedTeam.OffenseID));

                command.ExecuteNonQuery();
            }
        }
        //method that deletes all the teams a player is on if that player is deleted
        public void Delete(Guid playerID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete From Teams where OffenseID = @playerID or DefenseID = @playerID";
                command.Parameters.Add(new SQLiteParameter("@playerID", playerID));

                command.ExecuteNonQuery();
            }
        }
        public void Clear()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete From Teams ";

                command.ExecuteNonQuery();
            }


        }
    }
}


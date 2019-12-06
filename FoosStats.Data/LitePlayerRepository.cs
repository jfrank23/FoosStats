using FoosStats.Core;
using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace FoosStats.Data
{
    public class LitePlayerRepository : IPlayerRepository
    {
        readonly string connectionString = "Data Source= FoosData.db" + "; Version=3; BinaryGUID=False;";
        public LitePlayerRepository(string connectionString = null)
        {
            if (connectionString != null)
            {
                this.connectionString = connectionString;
            }
            CheckDatabaseExists();
        }

        private void CheckDatabaseExists()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "Select * from Players ";
                    var reader = command.ExecuteReader();
                    var player = new Player();
                    while (reader.Read())
                    {
                        player.ID = new Guid(reader.GetString(0));
                        player.FirstName = reader.GetString(1);
                        player.LastName = reader.GetString(2);
                        player.GamesPlayed = reader.GetInt32(3);
                        player.GamesWon = reader.GetInt32(4);
                        player.GamesLost = reader.GetInt32(5);
                        player.GoalsFor = reader.GetInt32(6);
                        player.GoalsAgainst = reader.GetInt32(7);

                    }
                }
            }
            catch
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"CREATE TABLE \"Games\" (\"GameId\" TEXT NOT NULL UNIQUE, \"BlueScore\" INTEGER, \"RedScore\"  INTEGER, \"GameTime\"  TEXT, \"RedOffense\"    TEXT, \"RedDefense\"    TEXT, \"BlueOffense\"   TEXT, \"BlueDefense\"   TEXT, PRIMARY KEY(\"GameId\"))";
                    command.ExecuteNonQuery();
                    var command2 = connection.CreateCommand();
                    command2.CommandText = $"CREATE TABLE \"Players\" ( \"PlayerId\" TEXT NOT NULL UNIQUE, \"FirstName\" TEXT, \"LastName\" TEXT, \"GamesPlayed\" INTEGER, \"GamesWon\" INTEGER, \"GamesLost\" INTEGER, \"GoalsFor\" INTEGER, \"GoalsAgainst\" INTEGER, PRIMARY KEY(\"PlayerId\") )";
                    command2.ExecuteNonQuery();
                    var command3 = connection.CreateCommand();
                    command3.CommandText = $"CREATE TRIGGER addGame AFTER INSERT ON Games BEGIN UPDATE Players SET GoalsFor = GoalsFor + New.BlueScore, GoalsAgainst = GoalsAgainst + New.RedScore, GamesPlayed = GamesPlayed + 1, GamesWon = CASE WHEN New.BlueScore = 10 THEN GamesWon +1 ELSE GamesWon +0 END, GamesLost = CASE WHEN New.RedScore = 10 THEN GamesLost +1 ELSE GamesLost +0 END WHERE PlayerId = New.BlueDefense OR PlayerId = New.BlueOffense; UPDATE Players SET GoalsFor = GoalsFor + New.RedScore, GoalsAgainst = GoalsAgainst + New.BlueScore, GamesPlayed = GamesPlayed + 1, GamesWon = CASE WHEN New.RedScore = 10 THEN GamesWon +1 ELSE GamesWon +0 END, GamesLost = CASE WHEN New.BlueScore = 10 THEN GamesLost +1 ELSE GamesLost +0 END WHERE PlayerId = New.RedDefense OR PlayerId = New.RedOffense; END";
                    command3.ExecuteNonQuery();
                    var command4 = connection.CreateCommand();
                    command4.CommandText = $"CREATE TRIGGER deleteGame AFTER DELETE ON Games BEGIN UPDATE Players SET GoalsFor = GoalsFor - Old.BlueScore, GoalsAgainst = GoalsAgainst - Old.RedScore, GamesPlayed = GamesPlayed - 1, GamesWon = CASE WHEN Old.BlueScore = 10 THEN GamesWon -1 ELSE GamesWon END, GamesLost = CASE WHEN Old.RedScore = 10 THEN GamesLost -1 ELSE GamesLost END WHERE PlayerId = Old.BlueDefense OR PlayerId = Old.BlueOffense; UPDATE Players SET GoalsFor = GoalsFor - Old.RedScore, GoalsAgainst = GoalsAgainst - Old.BlueScore, GamesPlayed = GamesPlayed - 1, GamesWon = CASE WHEN Old.RedScore = 10 THEN GamesWon -1 ELSE GamesWon +0  END, GamesLost = CASE WHEN Old.BlueScore = 10 THEN GamesLost -1 ELSE GamesLost +0  END WHERE PlayerId = Old.RedDefense OR PlayerId = Old.RedOffense; END";
                    command4.ExecuteNonQuery();
                }
            }
        }

        public Player Add(Player player)
        {
            player.ID = Guid.NewGuid();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Insert into Players(PlayerId, FirstName,LastName,GamesPlayed,GamesWon,GamesLost,GoalsFor,GoalsAgainst) values(@Id, @FirstName,@LastName,@GamesPlayed,@GamesWon,@GamesLost,@GoalsFor,@GoalsAgainst)";
                command.Parameters.Add(new SQLiteParameter("@Id", player.ID));
                command.Parameters.Add(new SQLiteParameter("@FirstName", player.FirstName));
                command.Parameters.Add(new SQLiteParameter("@LastName", player.LastName));
                command.Parameters.Add(new SQLiteParameter("@GamesPlayed", player.GamesPlayed));
                command.Parameters.Add(new SQLiteParameter("@GamesWon", player.GamesWon));
                command.Parameters.Add(new SQLiteParameter("@GamesLost", player.GamesLost));
                command.Parameters.Add(new SQLiteParameter("@GoalsFor", player.GoalsFor));
                command.Parameters.Add(new SQLiteParameter("@GoalsAgainst", player.GoalsAgainst));

                command.ExecuteNonQuery();
                return player;
            }
        }



        public void Delete(Guid playerID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete from Players where PlayerId = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", playerID));
                command.ExecuteNonQuery();
            }
        }

        public Player GetPlayerById(Guid id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Select * from Players where PlayerId = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", id));
                var reader = command.ExecuteReader();
                var player = new Player();
                while (reader.Read())
                {
                    player.ID = new Guid(reader.GetString(0));
                    player.FirstName = reader.GetString(1);
                    player.LastName = reader.GetString(2);
                    player.GamesPlayed = reader.GetInt32(3);
                    player.GamesWon = reader.GetInt32(4);
                    player.GamesLost = reader.GetInt32(5);
                    player.GoalsFor = reader.GetInt32(6);
                    player.GoalsAgainst = reader.GetInt32(7);

                }
                return player;
            }
        }

        public IEnumerable<Player> GetPlayersByName(string name = null)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var players = new List<Player>();
                var command = connection.CreateCommand();
                if (name == null)
                {
                    command.CommandText = "Select * from Players";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        players.Add(new Player
                        {
                            ID = new Guid(reader.GetString(0)),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            GamesPlayed = reader.GetInt32(3),
                            GamesWon = reader.GetInt32(4),
                            GamesLost = reader.GetInt32(5),
                            GoalsFor = reader.GetInt32(6),
                            GoalsAgainst = reader.GetInt32(7)
                        });
                    }
                }
                else
                {
                    command.CommandText = "Select * from Players";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetString(1).ToLower().Contains(name.ToLower()) || reader.GetString(2).ToLower().Contains(name.ToLower()))
                        {
                            players.Add(new Player
                            {
                                ID = new Guid(reader.GetString(0)),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                GamesPlayed = reader.GetInt32(3),
                                GamesWon = reader.GetInt32(4),
                                GamesLost = reader.GetInt32(5),
                                GoalsFor = reader.GetInt32(6),
                                GoalsAgainst = reader.GetInt32(7)
                            });
                        }
                    }
                }
                return players.OrderBy(r => r.LastName);
            }
        }

        public Player Update(Player player)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Update Players Set FirstName = @FirstName,LastName= @LastName,GamesPlayed= @GamesPlayed,GamesWon= @GamesWon,GamesLost= @GamesLost,GoalsFor= @GoalsFor,GoalsAgainst = @GoalsAgainst WHERE PlayerId = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", player.ID));
                command.Parameters.Add(new SQLiteParameter("@FirstName", player.FirstName));
                command.Parameters.Add(new SQLiteParameter("@LastName", player.LastName));
                command.Parameters.Add(new SQLiteParameter("@GamesPlayed", player.GamesPlayed));
                command.Parameters.Add(new SQLiteParameter("@GamesWon", player.GamesWon));
                command.Parameters.Add(new SQLiteParameter("@GamesLost", player.GamesLost));
                command.Parameters.Add(new SQLiteParameter("@GoalsFor", player.GoalsFor));
                command.Parameters.Add(new SQLiteParameter("@GoalsAgainst", player.GoalsAgainst));

                command.ExecuteNonQuery();
            }
            return player;
        }
    }
}

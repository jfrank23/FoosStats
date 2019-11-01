using FoosStats.Core;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace FoosStats.Data
{
    public class LitePlayerRepository : IPlayerRepository
    {
        readonly string connectionString = "Data Source= " + Environment.CurrentDirectory.Replace("\\FoosStats\\FoosStats", "\\FoosStats") + "\\FoosStats.Data\\FoosData.db" + "; Version=3; BinaryGUID=False;";
        public LitePlayerRepository(string connectionString = null)
        {
            if (connectionString != null)
            {
                this.connectionString = connectionString;
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
        
        public string ReturnPlayerNameFromId(Guid id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "Select * from Players where PlayerId = @Id";
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
                return $"{player.FirstName} {player.LastName}";

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

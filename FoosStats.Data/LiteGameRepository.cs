using FoosStats.Core;
using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace FoosStats.Data
{
    public class LiteGameRepository : IGameRepository
    {
        //Add and Delete games automatically triggers a response in the players table to update those stats. Look at the SQL files to see update information
        readonly string connectionString = "Data Source= " + "FoosData.db" + "; Version=3; BinaryGUID=False;";
        public LiteGameRepository(string connectionString = null)
        {
            if (connectionString != null)
            {
                this.connectionString = connectionString;
            }
        }
        public Game Add(Game newGame)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                newGame.GameID = Guid.NewGuid();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Insert into Games(GameId, BlueScore,RedScore,GameTime,RedOffense,RedDefense,BlueOffense,BlueDefense) values(@Id, @BlueScore,@RedScore,@GameTime,@RedOffense,@RedDefense,@BlueOffense,@BlueDefense)";
                command.Parameters.Add(new SQLiteParameter("@Id", newGame.GameID));
                command.Parameters.Add(new SQLiteParameter("@BlueScore", newGame.BlueScore));
                command.Parameters.Add(new SQLiteParameter("@RedScore", newGame.RedScore));
                command.Parameters.Add(new SQLiteParameter("@GameTime", newGame.GameTime.ToString()));
                command.Parameters.Add(new SQLiteParameter("@RedOffense", newGame.RedOffense));
                command.Parameters.Add(new SQLiteParameter("@RedDefense", newGame.RedDefense));
                command.Parameters.Add(new SQLiteParameter("@BlueOffense", newGame.BlueOffense));
                command.Parameters.Add(new SQLiteParameter("@BlueDefense", newGame.BlueDefense));


                command.ExecuteNonQuery();

            }
            return newGame;
        }

        public void Delete(Guid gameID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete from Games where GameId = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", gameID));
                command.ExecuteNonQuery();
            }
        }

        public DisplayGame GetGameByID(Guid gameID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT " +
                    "GameId, BlueScore,RedScore,GameTime, BlueDefense,BDFirst, BDLast,BlueOffense, BOFirst,BOLast,RedDefense,RDFirst,RDLast,RedOffense,ROFirst,ROLast " +
                    "From(SELECT * FROM Games " +
                    "LEFT JOIN(SELECT PlayerId as BDId,FirstName as BDFirst,LastName as BDLast from Players) ON BlueDefense = BDId " +
                    "LEFT JOIN(SELECT PlayerId as BOId,FirstName as BOFirst,LastName as BOLast from Players) ON BlueOffense = BOId " +
                    "LEFT JOIN(SELECT PlayerId as RDId,FirstName as RDFirst,LastName as RDLast from Players) ON RedDefense = RDId " +
                    "LEFT JOIN(SELECT PlayerId as ROId,FirstName as ROFirst, LastName as ROLast from Players) ON RedOffense = ROId)" +
                    "WHERE GameId = @Id; ";
                command.Parameters.Add(new SQLiteParameter("@Id", gameID));
                var reader = command.ExecuteReader();
                var game = new DisplayGame();
                while (reader.Read())
                {
                    game.GameID = new Guid(reader.GetString(0));
                    game.BlueScore = reader.GetInt32(1);
                    game.RedScore = reader.GetInt32(2);
                    game.GameTime = DateTime.Parse(reader.GetString(3));
                    game.BlueDefense = new Guid(reader.GetString(4));
                    game.BlueDefenseName = reader.GetValue(5).ToString() + " " + reader.GetValue(6).ToString();
                    game.BlueOffense = new Guid(reader.GetString(7));
                    game.BlueOffenseName = reader.GetValue(8).ToString() + " " + reader.GetValue(9).ToString();
                    game.RedDefense = new Guid(reader.GetString(10));
                    game.RedDefenseName = reader.GetValue(11).ToString() + " " + reader.GetValue(12).ToString();
                    game.RedOffense = new Guid(reader.GetString(13));
                    game.RedOffenseName = reader.GetValue(14).ToString() + " " + reader.GetValue(15).ToString();

                }
                return game;
            }
        }

        public IEnumerable<DisplayGame> GetGames()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var games = new List<DisplayGame>();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT " +
                    "GameId, BlueScore,RedScore,GameTime, BlueDefense,BDFirst, BDLast,BlueOffense, BOFirst,BOLast,RedDefense,RDFirst,RDLast,RedOffense,ROFirst,ROLast " +
                    "From(SELECT * FROM Games " +
                    "LEFT JOIN(SELECT PlayerId as BDId,FirstName as BDFirst,LastName as BDLast from Players) ON BlueDefense = BDId " +
                    "LEFT JOIN(SELECT PlayerId as BOId,FirstName as BOFirst,LastName as BOLast from Players) ON BlueOffense = BOId " +
                    "LEFT JOIN(SELECT PlayerId as RDId,FirstName as RDFirst,LastName as RDLast from Players) ON RedDefense = RDId " +
                    "LEFT JOIN(SELECT PlayerId as ROId,FirstName as ROFirst, LastName as ROLast from Players) ON RedOffense = ROId); ";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    games.Add(new DisplayGame
                    {
                        GameID = new Guid(reader.GetString(0)),
                        BlueScore = reader.GetInt32(1),
                        RedScore = reader.GetInt32(2),
                        GameTime = DateTime.Parse(reader.GetString(3)),
                        BlueDefense = new Guid(reader.GetString(4)),
                        BlueDefenseName = reader.GetValue(5).ToString() + " " + reader.GetValue(6).ToString(),
                        BlueOffense = new Guid(reader.GetString(7)),
                        BlueOffenseName = reader.GetValue(8).ToString() + " " + reader.GetValue(9).ToString(),
                        RedDefense = new Guid(reader.GetString(10)),
                        RedDefenseName = reader.GetValue(11).ToString() + " " + reader.GetValue(12).ToString(),
                        RedOffense = new Guid(reader.GetString(13)),
                        RedOffenseName = reader.GetValue(14).ToString() + " " + reader.GetValue(15).ToString()

                    });
                }

                return games.OrderByDescending(r => r.GameTime);
            }
        }


    }
}

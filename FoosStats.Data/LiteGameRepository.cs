using System;
using System.Collections.Generic;
using System.Data.SQLite;
using FoosStats.Core;
using System.Web;

namespace FoosStats.Data
{
    public class LiteGameRepository : IGameRepository
    {
        //Add and Delete games automatically triggers a response in the players table to update those stats. Look at the SQL files to see update information
        string connectionString = "Data Source= " + Environment.CurrentDirectory.Replace("\\FoosStats\\FoosStats","\\FoosStats") +"\\FoosStats.Data\\FoosData.db" + "; Version=3; BinaryGUID=False;";
        
        public Game Add(Game newGame)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Insert into Games(Id, BlueScore,RedScore,GameTime,RedOffense,RedDefense,BlueOffense,BlueDefense) values(@Id, @BlueScore,@RedScore,@GameTime,@RedOffense,@RedDefense,@BlueOffense,@BlueDefense)";
                command.Parameters.Add(new SQLiteParameter("@Id", Guid.NewGuid()));
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
                command.CommandText = $"Delete from Games where Id = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", gameID));
                command.ExecuteNonQuery();
            }
        }

        public Game GetGameByID(Guid gameID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Select * from Games where Id = @Id";
                command.Parameters.Add(new SQLiteParameter("@Id", gameID));
                var reader = command.ExecuteReader();
                var game = new Game();
                while (reader.Read())
                {
                    game.GameID = new Guid(reader.GetString(0));
                    game.BlueScore = reader.GetInt32(1);
                    game.RedScore = reader.GetInt32(2);
                    game.GameTime = DateTime.Parse(reader.GetString(3));
                    game.RedOffense = new Guid(reader.GetString(4));
                    game.RedDefense = new Guid(reader.GetString(5));
                    game.BlueOffense = new Guid(reader.GetString(6));
                    game.BlueDefense = new Guid(reader.GetString(7));

                }
                return game;
            }
        }

        public IEnumerable<Game> GetGames()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var games = new List<Game>();
                var command = connection.CreateCommand();
                command.CommandText = "Select * from Games";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    games.Add(new Game
                    {
                        GameID = new Guid(reader.GetString(0)),
                        BlueScore = reader.GetInt32(1),
                        RedScore = reader.GetInt32(2),
                        GameTime = DateTime.Parse(reader.GetString(3)),
                        RedOffense = new Guid(reader.GetString(4)),
                        RedDefense = new Guid(reader.GetString(5)),
                        BlueOffense = new Guid(reader.GetString(6)),
                        BlueDefense = new Guid(reader.GetString(7)),

                    });
                }
                return games;
            }
        }

        public Game Update(Game updatedGame)
        {
            Delete(updatedGame.GameID);
            Add(updatedGame);
            return updatedGame;
        }
    }
}

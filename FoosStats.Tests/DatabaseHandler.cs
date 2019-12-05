using FoosStats.Core;
using System.Data.SQLite;

namespace FoosStats.Tests
{
    public static class DatabaseHandler
    {
        public static void ClearAll(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"Delete from Players";
                command.ExecuteNonQuery();
                
                var command2 = connection.CreateCommand();
                command2.CommandText = $"Delete from Games";
                command2.ExecuteNonQuery();
            }
        }
        public static Player GetArbitraryPlayer(string suffix = null)
        {
            return new Player
            {
                FirstName = "FirstName" + suffix,
                LastName = "LastName" + suffix
            };
        }
    }
}

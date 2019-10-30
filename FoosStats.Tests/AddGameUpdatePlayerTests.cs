using FluentAssertions;
using FoosStats.Core;
using FoosStats.Data;
using NUnit.Framework;
using System;

namespace FoosStats.Tests
{
    [TestFixture]
    class AddGameUpdatePlayerTests
    {
        private readonly string connectionString = "Data Source= " + Environment.CurrentDirectory.Replace("\\bin\\Debug\\netcoreapp3.0", "") + "\\FoosTestData.db" + "; Version=3; BinaryGUID=False;";
        Player player1;
        Player player2;
        Player player3;
        Player player4;
        Game game;
        LiteGameRepository gameRepo;
        LitePlayerRepository playerRepo;

        [TearDown]
        public void TearDown()
        {
            DatabaseHandler.ClearAll(connectionString);
        }

        [SetUp]
        public void Setup()
        {
            gameRepo = new LiteGameRepository(connectionString);
            playerRepo = new LitePlayerRepository(connectionString);
            player1 = DatabaseHandler.GetArbitraryPlayer("1"); ;
            player2 = DatabaseHandler.GetArbitraryPlayer("2"); ;
            player3 = DatabaseHandler.GetArbitraryPlayer("3"); ;
            player4 = DatabaseHandler.GetArbitraryPlayer("4"); ;

            player1 = playerRepo.Add(player1);
            player2 = playerRepo.Add(player2);
            player3 = playerRepo.Add(player3);
            player4 = playerRepo.Add(player4);

            game = new Game
            {
                RedScore = 10,
                BlueScore = 9,
                BlueOffense = player1.ID,
                BlueDefense = player2.ID,
                RedOffense = player3.ID,
                RedDefense = player4.ID
            };


            player1.GamesPlayed = 1;
            player1.GamesLost = 1;
            player1.GamesWon = 0;
            player1.GoalsAgainst = 10;
            player1.GoalsFor = 9;

            player2.GamesPlayed = 1;
            player2.GamesLost = 1;
            player2.GamesWon = 0;
            player2.GoalsAgainst = 10;
            player2.GoalsFor = 9;


            player3.GamesPlayed = 1;
            player3.GamesLost = 0;
            player3.GamesWon = 1;
            player3.GoalsAgainst = 9;
            player3.GoalsFor = 10;

            player4.GamesPlayed = 1;
            player4.GamesLost = 0;
            player4.GamesWon = 1;
            player4.GoalsAgainst = 9;
            player4.GoalsFor = 10;
     

        }

        [Test]
        public void AddGameShouldUpdatePlayerTest()
        {
            game = gameRepo.Add(game);
            var resultPlayer1 = playerRepo.GetPlayerById(player1.ID);
            var resultPlayer2 = playerRepo.GetPlayerById(player2.ID);
            var resultPlayer3 = playerRepo.GetPlayerById(player3.ID);
            var resultPlayer4 = playerRepo.GetPlayerById(player4.ID);

            resultPlayer1.Should().BeEquivalentTo(player1);
            resultPlayer2.Should().BeEquivalentTo(player2);
            resultPlayer3.Should().BeEquivalentTo(player3);
            resultPlayer4.Should().BeEquivalentTo(player4);
        }
    }
}

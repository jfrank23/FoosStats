using FluentAssertions;
using FoosStats.Core;
using FoosStats.Data;
using NUnit.Framework;
using System;

namespace FoosStats.Tests
{
    [TestFixture]
    class DeleteGameUpdatePlayerTests
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

            game = gameRepo.Add(game);

        }

        [Test]
        public void DeleteGameShouldUpdatePlayerTest()
        {

            var resultPlayer1 = playerRepo.GetPlayerById(player1.ID);
            var resultPlayer2 = playerRepo.GetPlayerById(player2.ID);
            var resultPlayer3 = playerRepo.GetPlayerById(player3.ID);
            var resultPlayer4 = playerRepo.GetPlayerById(player4.ID);

            resultPlayer1.Should().NotBeEquivalentTo(player1);
            resultPlayer2.Should().NotBeEquivalentTo(player2);
            resultPlayer3.Should().NotBeEquivalentTo(player3);
            resultPlayer4.Should().NotBeEquivalentTo(player4);

            gameRepo.Delete(game.GameID);

            resultPlayer1 = playerRepo.GetPlayerById(player1.ID);
            resultPlayer2 = playerRepo.GetPlayerById(player2.ID);
            resultPlayer3 = playerRepo.GetPlayerById(player3.ID);
            resultPlayer4 = playerRepo.GetPlayerById(player4.ID);

            resultPlayer1.Should().BeEquivalentTo(player1);
            resultPlayer2.Should().BeEquivalentTo(player2);
            resultPlayer3.Should().BeEquivalentTo(player3);
            resultPlayer4.Should().BeEquivalentTo(player4);
        }
    }
}

using System;
using System.Collections.Generic;
using FoosStats.Core;
using System.Linq;

namespace FoosStats.Data
{
    public class TestGameRepo : IGameRepository
    {

        List<Game> games = new List<Game>
        {
            new Game
            {
                GameID = Guid.NewGuid(),
                BlueDefense = TestPlayerRepo.players[2].ID,
                BlueOffense =TestPlayerRepo.players[0].ID,
                RedDefense = TestPlayerRepo.players[3].ID,
                RedOffense = TestPlayerRepo.players[1].ID,
                BlueScore = 10,
                RedScore = 9,
                GameTime = DateTime.Now
            },
            new Game
            {
                GameID = Guid.NewGuid(),
                BlueDefense = TestPlayerRepo.players[2].ID,
                BlueOffense =TestPlayerRepo.players[0].ID,
                RedDefense = TestPlayerRepo.players[3].ID,
                RedOffense = TestPlayerRepo.players[1].ID,
                BlueScore = 7,
                RedScore = 10,
                GameTime = DateTime.Now.AddMinutes(10)
            },
        };
        public IEnumerable<Game> GetGames()
        {
            return (from game in games
                   orderby game.GameTime
                   select game).Reverse();
        }
        public Game Update(Game updatedGame)
        {
            var game = games.FirstOrDefault(r=> r.GameTime == updatedGame.GameTime);
            game.BlueDefense = updatedGame.BlueDefense;
            game.BlueOffense = updatedGame.BlueOffense;
            game.BlueScore = updatedGame.BlueScore;
            game.RedDefense = updatedGame.RedDefense;
            game.RedOffense = updatedGame.RedOffense;
            game.RedScore = updatedGame.RedScore;
            game.GameTime = updatedGame.GameTime;
            return game;
        }
        public Game Add(Game newGame)
        {
            games.Add(newGame);
            return newGame;
        }

        public Game GetGameByID(Guid gameID)
        {
            var game = games.FirstOrDefault(r=>Guid.Equals(r.GameID,gameID));
            
            return game;
        }

        public void Delete(Guid gameID)
        {
            games.Remove(GetGameByID(gameID));
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Creators
{
    public class GameCreator :ICreator<Game>
    {
        private IGameRepository gameRepository;
        private ILeaderboards leaderboard;
        public GameCreator(IGameRepository gameRepository, ILeaderboards leaderboard)
        {
            this.gameRepository = gameRepository;
            this.leaderboard = leaderboard;
        }
        public Game Create(Game game)
        {
            return gameRepository.Add(game);
        }
    }
}

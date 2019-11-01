using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Creators
{
    public class GameCreator :ICreator<Game>
    {
        private IGameRepository gameRepository;
        public GameCreator(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        public Game Create(Game game)
        {
            return gameRepository.Add(game);
        }
    }
}

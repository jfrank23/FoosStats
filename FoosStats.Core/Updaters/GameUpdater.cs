using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Updaters
{
    public class GameUpdater:IUpdater<Game>
    {
        private IGameRepository gameRepository;
        public GameUpdater(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        public Game Update(Game game)
        {
            return gameRepository.Update(game);
        }
    }
}

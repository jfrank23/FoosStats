using FoosStats.Core.Creators;
using FoosStats.Core.Deleters;
using FoosStats.Core.Repositories;

namespace FoosStats.Core.Updaters
{
    public class GameUpdater:IUpdater<Game>
    {
        private readonly ICreator<Game> gameCreator;
        private readonly IDeleter<Game> gameDeleter;

        public GameUpdater(ICreator<Game> gameCreator, IDeleter<Game> gameDeleter)
        {
            this.gameCreator = gameCreator;
            this.gameDeleter = gameDeleter;
        }
        public Game Update(Game game)
        {
            gameDeleter.Delete(game.GameID);
            gameCreator.Create(game);

            return game;
             
        }
    }
}

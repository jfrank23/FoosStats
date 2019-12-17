using FoosStats.Core.Creators;
using FoosStats.Core.Deleters;
using FoosStats.Core.Repositories;

namespace FoosStats.Core.Updaters
{
    public class GameUpdater:IUpdater<Game>
    {
        private readonly ICreator<Game> gameCreator;
        private readonly IDeleter<Game> gameDeleter;
        private readonly ITeamUpdater teamUpdater;

        public GameUpdater(ICreator<Game> gameCreator, IDeleter<Game> gameDeleter, ITeamUpdater teamUpdater)
        {
            this.gameCreator = gameCreator;
            this.gameDeleter = gameDeleter;
            this.teamUpdater = teamUpdater;
        }
        public Game Update(Game game)
        {
            gameDeleter.Delete(game.GameID);
            gameCreator.Create(game);
            teamUpdater.Refresh();

            return game;
             
        }
    }
}

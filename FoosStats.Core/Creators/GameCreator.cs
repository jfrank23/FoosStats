using FoosStats.Core.Repositories;
using FoosStats.Core.Updaters;

namespace FoosStats.Core.Creators
{
    public class GameCreator :ICreator<Game>
    {
        private IGameRepository gameRepository;
        private readonly IPlayerUpdater playerUpdater;

        public GameCreator(IGameRepository gameRepository, IPlayerUpdater playerUpdater)
        {
            this.gameRepository = gameRepository;
            this.playerUpdater = playerUpdater;
        }
        public Game Create(Game game)
        {
            playerUpdater.UpdatePlayerAfterAddGame(game);
            return gameRepository.Add(game);
        }
    }
}

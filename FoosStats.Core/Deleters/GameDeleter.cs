using FoosStats.Core.Repositories;
using FoosStats.Core.Updaters;
using System;

namespace FoosStats.Core.Deleters
{
    public class GameDeleter : IDeleter<Game>
    {
        private IGameRepository gameRepository;
        private readonly IPlayerUpdater playerUpdater;

        public GameDeleter(IGameRepository gameRepository, IPlayerUpdater playerUpdater)
        {
            this.gameRepository = gameRepository;
            this.playerUpdater = playerUpdater;
        }
        public void Delete(Guid gameID)
        {
            playerUpdater.UpdatePlayerAfterDeleteGame(gameRepository.GetGameByID(gameID));
            gameRepository.Delete(gameID);
        }
    }
}

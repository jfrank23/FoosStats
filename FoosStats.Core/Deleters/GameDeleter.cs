using FoosStats.Core.Repositories;
using FoosStats.Core.Updaters;
using System;

namespace FoosStats.Core.Deleters
{
    public class GameDeleter : IDeleter<Game>
    {
        private IGameRepository gameRepository;
        private readonly IPlayerUpdater playerUpdater;
        private readonly ITeamUpdater teamUpdater;

        public GameDeleter(IGameRepository gameRepository, IPlayerUpdater playerUpdater, ITeamUpdater teamUpdater)
        {
            this.gameRepository = gameRepository;
            this.playerUpdater = playerUpdater;
            this.teamUpdater = teamUpdater;
        }
        public void Delete(Guid gameID)
        {
            playerUpdater.UpdatePlayerAfterDeleteGame(gameRepository.GetGameByID(gameID));
            gameRepository.Delete(gameID);
            teamUpdater.Refresh();

        }
    }
}

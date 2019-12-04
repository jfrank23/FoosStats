using System;

namespace FoosStats.Core.Deleters
{
    public class PlayerDeleter : IDeleter<Player>
    {
        private IPlayerRepository playerRepository;
        public PlayerDeleter(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public void Delete(Guid playerID)
        {
            playerRepository.Delete(playerID);
        }
    }
}

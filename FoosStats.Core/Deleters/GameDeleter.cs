using System;

namespace FoosStats.Core.Deleters
{
    public class GameDeleter : IDeleter<Game>
    {
        private IGameRepository gameRepository;
        public GameDeleter(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        public void Delete(Guid gameID)
        {
            gameRepository.Delete(gameID);
        }
    }
}

using System;
using System.Collections.Generic;

namespace FoosStats.Core.Retrievers
{
    public interface IGameRetriever
    {
        IEnumerable<DisplayGame> GetAllGames();
        Game GetGameById(Guid gameID);
    }
    public class GameRetriever : IGameRetriever
    {
        private IGameRepository gameRepository;
        public GameRetriever(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        public Game GetGameById(Guid gameID)
        {
            return gameRepository.GetGameByID(gameID);
        }
        public IEnumerable<DisplayGame> GetAllGames()
        {
            return gameRepository.GetGames();
        }
        
    }
}

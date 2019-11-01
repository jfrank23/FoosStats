using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Retrievers
{
    public interface IPlayerRetriever
    {
        Player GetPlayerById(Guid playerID);
        IEnumerable<Player> GetPlayersByName(string name = null);
    }
    public class PlayerRetriever : IPlayerRetriever
    {
        private IPlayerRepository playerRepository;
        public PlayerRetriever(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public Player GetPlayerById(Guid playerID)
        {
            return playerRepository.GetPlayerById(playerID);
        }
        public IEnumerable<Player> GetPlayersByName(string name = null)
        {
            return playerRepository.GetPlayersByName(name);
        }
    }
}

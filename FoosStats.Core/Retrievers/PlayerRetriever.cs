using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;

namespace FoosStats.Core.Retrievers
{
    public interface IPlayerRetriever
    {
        Player GetPlayerById(Guid playerID);
        IEnumerable<Player> GetPlayers();
        string GuidToName(Guid Id);
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
        public IEnumerable<Player> GetPlayers()
        {
            return playerRepository.GetPlayers();
        }
        public string GuidToName(Guid Id)
        {

            var player = GetPlayerById(Id);
            return (player.FirstName + " " + player.LastName);

        }
    }
}

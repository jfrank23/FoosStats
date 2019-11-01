using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Creators
{
    public class PlayerCreator :ICreator<Player>
    {
        private IPlayerRepository playerRepository;
        public PlayerCreator(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public Player Create(Player player)
        {
            return playerRepository.Add(player);
        }
    }
}

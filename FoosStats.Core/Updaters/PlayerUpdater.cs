using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Updaters
{
    public class PlayerUpdater : IUpdater<Player>
    {
        private IPlayerRepository playerRepository;
        public PlayerUpdater(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public Player Update(Player player)
        {
            return playerRepository.Update(player);
        }
    }
}

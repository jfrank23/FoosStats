
using System;
using System.Collections.Generic;

namespace FoosStats.Core.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetPlayers();
        Player GetPlayerById(Guid id);
        Player Update(Player player);
        Player Add(Player player);
        void Delete(Guid playerID);
    }
}

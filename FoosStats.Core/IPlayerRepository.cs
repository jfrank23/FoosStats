
using System;
using System.Collections.Generic;

namespace FoosStats.Core
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetPlayersByName(string name = null);
        Player GetPlayerById(Guid id);
        Player Update(Player player);
        Player Add(Player player);
        void Delete(Guid playerID);
    }
}

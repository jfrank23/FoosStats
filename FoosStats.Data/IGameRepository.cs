using System;
using System.Collections.Generic;
using System.Text;
using FoosStats.Core;

namespace FoosStats.Data
{
    public interface IGameRepository
    {
        Game Add(Game newGame);
        IEnumerable<Game> GetGames();
        Game Update(Game updatedGame);
        Game GetGameByID(Guid gameID);
        void Delete(Guid gameID);
    }
}

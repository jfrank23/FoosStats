using System;
using System.Collections.Generic;


namespace FoosStats.Core.Repositories
{
    public interface IGameRepository
    {
        Game Add(Game newGame);
        IEnumerable<DisplayGame> GetGames();
        Game Update(Game updatedGame);
        DisplayGame GetGameByID(Guid gameID);
        void Delete(Guid gameID);
    }
}

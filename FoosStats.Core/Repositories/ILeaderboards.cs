using System.Collections.Generic;
namespace FoosStats.Core.Repositories
{
    public interface ILeaderboards
    {
        IEnumerable<DerivedPlayerData> GetLeaderboard();
        void MakeLeaderboard();
        void Update();
    }

}
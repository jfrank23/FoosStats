using System.Collections.Generic;
namespace FoosStats.Core.Repositories
{
    public interface ILeaderboards
    {
        IEnumerable<DerivedPlayerData> GetFullLeaderboard();
        IEnumerable<DerivedPlayerData> GetLimitedLeaderboard();
        void MakeLeaderboard();
        void Update();
    }

}
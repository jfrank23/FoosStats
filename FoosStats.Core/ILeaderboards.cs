using System.Collections.Generic;
namespace FoosStats.Core
{
    public interface ILeaderboards
    {
        IEnumerable<DerivedPlayerData> GetLeaderboard();
        void MakeLeaderboard();
        void Update();
    }

}
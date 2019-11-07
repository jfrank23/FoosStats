using System.Collections.Generic;
namespace FoosStats.Core
{
    public interface ILeaderboards
    {
        IEnumerable<DerivedData> GetLeaderboard();
        void MakeLeaderboard();
        void Update();
    }

}
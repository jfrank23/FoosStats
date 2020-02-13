using System;
using System.Collections.Generic;
namespace FoosStats.Core.Repositories
{
    public interface ILeaderboards
    {
        DerivedPlayerData GetDataById(Guid Id);
        IEnumerable<DerivedPlayerData> GetFullLeaderboard();
        IEnumerable<DerivedPlayerData> GetLimitedLeaderboard();
        PlayerStandingData GetPlayerStandings(Guid Id);
        IEnumerable<DerivedPlayerData> MakeLeaderboard();
        void Update();
    }

}
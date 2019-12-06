using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core.Repositories
{
    public interface IHistoricalData
    {
        void Generate();
        Dictionary<string, List<int>> GetHistoricalTeamRanks();
    }
}

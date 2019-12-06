using FoosStats.Core.ELO;
using FoosStats.Core.Repositories;
using FoosStats.Core.Retrievers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoosStats.Core.PageSpecific
{

    public class TeamDetailRetriever
    {
        private readonly IGameRetriever gameRetriever;
        private readonly ITeamRetriever teamRetriever;
        private readonly IHistoricalData historicalData;

        public TeamDetailRetriever(IGameRetriever gameRetriever, ITeamRetriever teamRetriever, IHistoricalData historicalData)
        {
            this.gameRetriever = gameRetriever;
            this.teamRetriever = teamRetriever;
            this.historicalData = historicalData;
        }
        public List<int> GetHistoricalEloForTeam(Guid teamID)
        {
            var team = teamRetriever.GetTeamById(teamID);
            return historicalData.GetHistoricalTeamRanks()[$"{team.DefenseID} {team.OffenseID}"];
        }
        public IEnumerable<DisplayGame> GetGamesInvolved(Guid teamID)
        {
            var team = teamRetriever.GetTeamById(teamID);
            return gameRetriever.GetAllGames().Where(g => (g.BlueDefense == team.DefenseID && g.BlueOffense == team.OffenseID) || (g.RedDefense==team.DefenseID && g.RedOffense == team.OffenseID));
        }
    }
}

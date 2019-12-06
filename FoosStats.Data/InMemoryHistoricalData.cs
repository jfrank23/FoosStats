using FoosStats.Core;
using FoosStats.Core.ELO;
using FoosStats.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoosStats.Data
{
    public class InMemoryHistoricalData : IHistoricalData
    {
        private readonly ITeamRepository teamRepository;
        private readonly IGameRepository gameRepository;
        public Dictionary<string, List<int>> HistoricalTeamRanks;
        public InMemoryHistoricalData(ITeamRepository teamRepository, IGameRepository gameRepository)
        {
            HistoricalTeamRanks = new Dictionary<string, List<int>>();
            this.teamRepository = teamRepository;
            this.gameRepository = gameRepository;
        }
        public string TeamToKey(Team team)
        {
            return $"{team.DefenseID} {team.OffenseID}";
        }
        public void Generate()
        {
            HistoricalTeamRanks = new Dictionary<string, List<int>>();
            foreach(var team in teamRepository.GetTeams())
            {
                HistoricalTeamRanks.Add(TeamToKey(team), new List<int> { EloHandler.StartingScore });
            }
            foreach(var game in gameRepository.GetGames().OrderBy(g => g.GameTime))
            {
                var blueTeam = new DisplayTeam
                {
                    DefenseID = game.BlueDefense,
                    OffenseID = game.BlueOffense,
                    
                };
                var redTeam = new DisplayTeam
                {
                    DefenseID = game.RedDefense,
                    OffenseID = game.RedOffense
                };

                blueTeam.GamesPlayed = HistoricalTeamRanks[TeamToKey(blueTeam)].Count - 1;
                redTeam.GamesPlayed = HistoricalTeamRanks[TeamToKey(redTeam)].Count - 1;


                var blueRankList = HistoricalTeamRanks[TeamToKey(blueTeam)];
                var redRankList = HistoricalTeamRanks[TeamToKey(redTeam)];

                blueTeam.Rank = blueRankList[blueRankList.Count - 1];
                redTeam.Rank = redRankList[redRankList.Count - 1];

                var updatedRanks = EloHandler.UpdatedRanks(blueTeam, redTeam, game);

                HistoricalTeamRanks[TeamToKey(blueTeam)].Add(updatedRanks[0]);
                HistoricalTeamRanks[TeamToKey(redTeam)].Add(updatedRanks[1]);
            }
            //var blueIDs = $"{blueTeam.DefenseID} {blueTeam.OffenseID}";
            //var redIDs = $"{blueTeam.DefenseID} {blueTeam.OffenseID}";
            //try
            //{
            //    HistoricalTeamRanks[blueIDs].Add(blueTeam.Rank);
            //}
            //catch
            //{
            //    HistoricalTeamRanks.Add(blueIDs, new List<int> { 1200,blueTeam.Rank });
            //}
            //try
            //{
            //    HistoricalTeamRanks[redIDs].Add(redTeam.Rank);
            //}
            //catch
            //{
            //    HistoricalTeamRanks.Add(redIDs, new List<int> { 1200,redTeam.Rank });
            //}
        }
        public Dictionary<string, List<int>> GetHistoricalTeamRanks()
        {
            Generate();
            return HistoricalTeamRanks;
        }
    }


}

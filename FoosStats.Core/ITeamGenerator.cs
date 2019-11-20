using System;
using System.Collections.Generic;
using System.Linq;
using FoosStats.Core.Retrievers;

namespace FoosStats.Core
{
    public interface ITeamGenerator
    {
        IEnumerable<Player> players { get; set; }

        public List<List<String>> Randomize(IEnumerable<Guid> selectedPlayers)
;
    }
    public class TeamGenerator : ITeamGenerator
    {
        public IEnumerable<Player> players { get; set; }
        private IPlayerRetriever playerRetriever;
        private Random random = new Random();


        public TeamGenerator(IPlayerRetriever playerRetriever)
        {
            this.playerRetriever = playerRetriever;
            players = playerRetriever.GetPlayersByName();
        }

        public List<List<String>> Randomize(IEnumerable<Guid> selectedPlayers)
        {
            var unShuffled = selectedPlayers.ToList();
            var shuffled = new List<Guid>();
            while (unShuffled.Count() > 0)
            {
                var index = random.Next(unShuffled.Count());
                var player = unShuffled[index];
                shuffled.Add(player);
                unShuffled.Remove(player);
            }
            return SplitIntoTeams(shuffled);
        }

        private List<List<string>> SplitIntoTeams(List<Guid> shuffled)
        {
            //Team indicies are red=0, blue =1, bench=2
            var teams = new[] { new List<string>(), new List<string>(), new List<string>() };
            if (shuffled.Count() == 1)
            {
                teams[2].Add(playerRetriever.GuidToName(shuffled[0]));
                return teams.ToList();
            }
            else if (shuffled.Count() < 4)
            {
                var count = 0;
                foreach (var playerId in shuffled)
                {
                    teams[count].Add(playerRetriever.GuidToName(playerId));
                    count += 1;
                }
            }
            else
            {
                foreach (var playerId in shuffled)
                {
                    if (teams[0].Count() < 2)
                    {
                        teams[0].Add(playerRetriever.GuidToName(playerId));
                    }
                    else if (teams[1].Count() < 2)
                    {
                        teams[1].Add(playerRetriever.GuidToName(playerId));
                    }
                    else
                    {
                        teams[2].Add(playerRetriever.GuidToName(playerId));
                    }

                }
            }
            return teams.ToList();
        }
    }
}
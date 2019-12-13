using System;
using System.Collections.Generic;
using System.Linq;
using FoosStats.Core.Retrievers;

namespace FoosStats.Core.PageSpecific
{
    public interface ITeamGenerator
    {
        IEnumerable<Player> players { get; set; }

        List<DisplayTeam> FairTeams(IEnumerable<Guid> selectedPlayers);
        public List<List<String>> RandomTeams(IEnumerable<Guid> selectedPlayers);
    }
    public class TeamGenerator : ITeamGenerator
    {
        public IEnumerable<Player> players { get; set; }
        public ITeamRetriever teamRetriever { get; }
        public IEnumerable<DisplayTeam> teams;
        private IPlayerRetriever playerRetriever;
        private Random random;


        public TeamGenerator(IPlayerRetriever playerRetriever, ITeamRetriever teamRetriever)
        {
            this.playerRetriever = playerRetriever;
            teams = teamRetriever.GetAllTeams();
            players = playerRetriever.GetPlayers();
        }

        public List<List<String>> RandomTeams(IEnumerable<Guid> selectedPlayers)
        {
            random = new Random();
            List<Guid> shuffled = Shuffle(selectedPlayers);
            return SplitIntoTeams(shuffled);
        }
        public List<DisplayTeam> FairTeams(IEnumerable<Guid> selectedPlayers)
        {
            random = new Random();
            var selectedPlayersList = selectedPlayers.ToList();
            var shuffled = selectedPlayers;
            if (selectedPlayers.Count() < 4)
            {
                return new List<DisplayTeam>();
            }
            if (selectedPlayers.Count() > 4)
            {
                shuffled = Shuffle(selectedPlayers);
            }
            var matchup = new List<DisplayTeam>();
            var minDifference = 10000;
            var minPerm = Enumerable.Empty<Guid>();
            foreach (var perm in permute4(shuffled))
            {
                var blueTeam = teams.FirstOrDefault(t => (t.DefenseID == perm[0]) && (t.OffenseID == perm[1]));
                var redTeam = teams.FirstOrDefault(t => (t.DefenseID == perm[2]) && (t.OffenseID == perm[3]));
                if ((blueTeam == null))
                {
                    blueTeam = new DisplayTeam
                    {
                        DefenseName = playerRetriever.GuidToName(perm[0]),
                        OffenseName = playerRetriever.GuidToName(perm[1]),
                        Rank = ELO.EloHandler.StartingScore
                    };
                }
                if ((redTeam == null))
                {
                    redTeam = new DisplayTeam
                    {
                        DefenseName = playerRetriever.GuidToName(perm[2]),
                        OffenseName = playerRetriever.GuidToName(perm[3]),
                        Rank = ELO.EloHandler.StartingScore
                    };
                }
                var difference = Math.Abs(blueTeam.Rank + 100 - redTeam.Rank); //Blue Team with blue advantage minus redd rank
                if (difference < minDifference)
                {
                    minDifference = difference;
                    matchup = new List<DisplayTeam> { blueTeam, redTeam };
                    minPerm = perm;
                }
            }
            if(minDifference == 10000) { return new List<DisplayTeam>(); }
            foreach (var id in minPerm)
            {
                selectedPlayersList.Remove(id);
            }
            foreach(var id in selectedPlayersList)
            {
                matchup.Add(new DisplayTeam { DefenseName = playerRetriever.GuidToName(id)});
            }
            return matchup;

        }
        private List<Guid> Shuffle(IEnumerable<Guid> selectedPlayers)
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

            return shuffled;
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

        
        private List<List<Guid>> permute4(IEnumerable<Guid> selectedPlayers)
        {
            var allPermuted = new List<List<Guid>>();
            foreach (var id1 in selectedPlayers)
            {
                foreach (var id2 in selectedPlayers)
                {
                    foreach (var id3 in selectedPlayers)
                    {
                        foreach (var id4 in selectedPlayers)
                        {

                            var permutation = new List<Guid> { id1, id2, id3, id4 };
                            var groups = permutation.GroupBy(i=>i);
                            if (groups.Count() < 4)
                            {
                                continue;
                            }
                            allPermuted.Add(permutation);
                        }
                    }
                }
            }
            return allPermuted;
        }

    }
}
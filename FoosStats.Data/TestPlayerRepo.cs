using System;
using System.Collections.Generic;
using System.Linq;
using FoosStats.Core;

namespace FoosStats.Data
{
    public class TestPlayerRepo : IPlayerRepository
    {
        public static readonly List<Player> players = new List<Player>
        {
            new Player
            {
                ID = Guid.NewGuid(),
                FirstName = "Jordan",
                LastName = "Franklin",
                GamesPlayed = 10,
                GamesWon = 5,
                GamesLost = 5,
                GoalsFor = 45,
                GoalsAgainst = 30
            },
            new Player
            {
                ID = Guid.NewGuid(),
                FirstName = "Bradley",
                LastName = "Landis",
                GamesPlayed = 10,
                GamesWon = 6,
                GamesLost = 4,
                GoalsFor = 35,
                GoalsAgainst = 50
            },
            new Player
            {
                ID = Guid.NewGuid(),
                FirstName = "Nick",
                LastName = "Cianci",
                GamesPlayed = 10,
                GamesWon = 7,
                GamesLost = 3,
                GoalsFor = 45,
                GoalsAgainst = 45
            },
            new Player
            {
                ID = Guid.NewGuid(),
                FirstName = "Matt",
                LastName = "Myers",
                GamesPlayed = 10,
                GamesWon = 7,
                GamesLost = 3,
                GoalsFor = 50,
                GoalsAgainst = 30
            }
        };

        public Player GetPlayerById(Guid id)
        {
            var result = players.FirstOrDefault(r => Guid.Equals(r.ID, id));

            return result;
        }

        public string ReturnPlayerNameFromId(Guid id)
        {
            var player = GetPlayerById(id);
            return $"{player.FirstName} {player.LastName}";
        }

        public IEnumerable<Player> GetPlayersByName(string name = null)
        {
            return from player in players
                where string.IsNullOrEmpty(name) || player.LastName.StartsWith(name) || player.FirstName.StartsWith(name)
                orderby player.LastName
                select player;
        }

        public Player Update(Player updatedPlayer)
        {
            var player = players.SingleOrDefault(r => Guid.Equals(r.ID, updatedPlayer.ID));
            if (player != null)
            {
                player.FirstName = updatedPlayer.FirstName;
                player.LastName = updatedPlayer.LastName;
            }
            return player;

        }

        public Player Add(Player newPlayer)
        {
            players.Add(newPlayer);
            newPlayer.ID = Guid.NewGuid();
            return newPlayer;
        }


        public void Delete(Guid playerID)
        {
            players.Remove(GetPlayerById(playerID));
        }
    }
}
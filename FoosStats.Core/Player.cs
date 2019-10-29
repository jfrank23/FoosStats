using System;
using System.ComponentModel.DataAnnotations;

namespace FoosStats.Core
{
    public class Player
    {
        public Guid ID { get; set; }

        [Required, StringLength(80)]
        public string FirstName { get; set; }
        [Required, StringLength(80)]
        public string LastName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }


    }
}

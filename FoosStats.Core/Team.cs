using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public Guid DefenseID { get; set; }
        public Guid OffenseID { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int Rank { get; set; }

    }
    public class DisplayTeam: Team
    {
        public string DefenseName { get; set; }
        public string OffenseName { get; set; }
        public float WinPct { get; set; }
    }
}

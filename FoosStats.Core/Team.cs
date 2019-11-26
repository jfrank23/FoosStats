using System;
using System.Collections.Generic;
using System.Text;

namespace FoosStats.Core
{
    public class Team
    {
        public Player Defense { get; set; }
        public Player Offense { get; set; }
        public int GamesPlayed { get; set; }
        public float WinPct { get; set; }

    }
}

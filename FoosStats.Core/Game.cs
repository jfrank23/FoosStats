using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace FoosStats.Core
{
    public class Game
    {
        public Guid GameID { get; set; }
        public DateTime GameTime { get; set; }
        public Guid RedOffense { get; set; }
        public Guid RedDefense { get; set; }
        public Guid BlueOffense { get; set; }
        public Guid BlueDefense { get; set; }
        public int RedScore { get; set; }
        public int BlueScore { get; set; }
    }
}

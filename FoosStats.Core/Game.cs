using System;

namespace FoosStats.Core
{
    public class Game
    {
        public Game()
        {
            GameTime = DateTime.Now;
        }
        public Guid GameID { get; set; }
        public DateTime GameTime { get; set; }
        public Guid RedOffense { get; set; }
        public Guid RedDefense { get; set; }
        public Guid BlueOffense { get; set; }
        public Guid BlueDefense { get; set; }
        public int RedScore { get; set; }
        public int BlueScore { get; set; }
    }
    public class DisplayGame : Game
    {
        public string RedOffenseName{ get; set; }
        public string RedDefenseName { get; set; }
        public string BlueOffenseName { get; set; }
        public string BlueDefenseName { get; set; }
        public Game ToGame()
        {
            return new Game
            {
                GameID = this.GameID,
                GameTime = this.GameTime,
                RedOffense = this.RedOffense,
                RedDefense = this.RedDefense,
                BlueOffense = this.BlueOffense,
                BlueDefense = this.BlueDefense,
                RedScore = this.RedScore,
                BlueScore = this.BlueScore
            };
        }
    }
}


using System.Collections.Generic;

namespace Test
{
    public class Game
    {
        public int ID { get; set; }
        public string ScoringPlays { get; set; }
        public string NHLID { get; set; }

        public virtual ICollection<Play> Plays { get; set; }
    }
}

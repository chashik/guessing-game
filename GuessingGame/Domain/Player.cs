using System;

namespace GuessingGame.Domain
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public double GuessScore { get; set; }
        public int GamesPlayed { get; set; }
        public int GuessesMade { get; set; }
        public double Score
        {
            get
            {
                if (GamesPlayed == 0)
                    return 0;
                else
                    return Math.Round(
                        GuessScore / GamesPlayed,
                        3,
                        MidpointRounding.AwayFromZero);
            }
        }

        
    }
}

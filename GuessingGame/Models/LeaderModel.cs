using System.ComponentModel;

namespace GuessingGame.Models
{
    public class LeaderModel
    {
        [DisplayName("Player name(s)*")]
        public string Name { get; set; }
        
        [DisplayName("Score: total points divided by games played")]
        public double Score { get; set; }

        [DisplayName("Games Played")]
        public int GamesPlayed { get; set; }

        [DisplayName("Guesses Made")]
        public int GuessesMade { get; set; }

        [DisplayName("Matching digits but not on the right places: match = 0.5 point")]
        public int MatchesWithoutPosition { get; set; }

        [DisplayName("Matching digits on exact places: match = 1 point")]
        public int MatchesWithPosition { get; set; }
    }
}
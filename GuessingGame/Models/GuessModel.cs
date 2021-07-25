using System.ComponentModel;

namespace GuessingGame.Models
{
    public class GuessModel
    {
        public string FirstDigit { get; set; }
        public string SecondDigit { get; set; }
        public string ThirdDigit { get; set; }
        public string FourthDigit { get; set; }
        
        [DisplayName("Matching digits but not on the right places: ")]
        public int MatchesWithoutPosition { get; set; }

        [DisplayName("Matching digits on exact places: ")]
        public int MatchesWithPosition { get; set; }
    }
}

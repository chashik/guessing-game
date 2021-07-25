namespace GuessingGame.Domain
{
    public class GuessScore
    {
        public int GuessId { get; set; }
        public double Score { get; set; }
        public int MatchesWithoutPosition { get; set; }
        public int MatchesWithPosition { get; set; }
    }
}
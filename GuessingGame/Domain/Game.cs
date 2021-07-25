namespace GuessingGame.Domain
{
    public class Game : BaseEntity
    {
        public int PlayerId { get; set; }
        
        public string Secret { get; set; }

        public int BestGuessId { get; set; }

        public bool IsOver { get; set; }

        public int MatchesWithoutPosition { get; set; }

        public int MatchesWithPosition { get; set; }
    }
}

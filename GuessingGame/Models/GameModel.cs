using System.Collections.Generic;
using System.ComponentModel;

namespace GuessingGame.Models
{
    public class GameModel : IAuthorizable
    {
        [DisplayName("Player name: ")]
        public string PlayerName { get; set; }
        
        [DisplayName("Tries left: ")]
        public int TriesLeft { get; set; }

        [DisplayName("Enter your gess: ")]
        public GuessModel CurrentGuess { get; set; }

        [DisplayName("Previous try: ")]
        public GuessModel PreviousTry { get; set; }

        [DisplayName("Best result: ")]
        public GuessModel BestResult { get; set; }

        [DisplayName("Guess log: ")]
        public IReadOnlyList<GuessModel> GuessLog { get; set; }
        [DisplayName("Secret: ")]
        public string Secret { get; set; }
        public double Score { get; set; }
        public bool IsAuthorized { get; set; } = true;
    }
}

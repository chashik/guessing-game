namespace GuessingGame.Domain
{
    public class Guess : BaseEntity
    {
        public int GameId { get; set; }

        public int FirstDigit { get; set; }

        public int SecondDigit { get; set; }

        public int ThirdDigit { get; set; }

        public int FourthDigit { get; set; }
    }
}

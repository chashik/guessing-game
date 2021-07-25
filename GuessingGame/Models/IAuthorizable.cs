namespace GuessingGame.Models
{
    public interface IAuthorizable
    {
        bool IsAuthorized { get; set; }

        string PlayerName { get; set; }
    }
}

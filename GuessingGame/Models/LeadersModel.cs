using System.Collections.Generic;

namespace GuessingGame.Models
{
    public class LeadersModel : IAuthorizable
    {
        public bool IsAuthorized { get; set; } = true;
        public string PlayerName { get; set; }

        public IReadOnlyList<LeaderModel> Leaders { get; set; }
    }
}
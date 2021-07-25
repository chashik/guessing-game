using System.ComponentModel;

namespace GuessingGame.Models
{
    public class LoginModel
    {
        
        [DisplayName("Enter Your Name: ")]
        public string PlayerName { get; set; }
    }
}

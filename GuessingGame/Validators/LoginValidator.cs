using FluentValidation;
using GuessingGame.Models;

namespace GuessingGame.Validators
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(p => p.PlayerName)
                .NotEmpty().WithMessage("Provide a valid player name")
                .MinimumLength(4).WithMessage("Minimum 4 symbols")
                .MaximumLength(40).WithMessage("Maximum 40 symbols")
                .Matches("^[a-zA-Z0-9-_]+$").WithMessage("Use latin symbols, numbers, -_ signs");

            
            // move to unit test for server-side validation
            //RuleFor(p => p.PlayerName).Must((model, context) => 
            //{
            //    if (model.PlayerName.Contains("x"))
            //        return false;
            //    else
            //        return true;
            //}).WithMessage("Do not include x!");
        }
    }
}

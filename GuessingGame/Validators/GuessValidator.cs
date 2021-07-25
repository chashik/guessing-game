using FluentValidation;
using GuessingGame.Models;

namespace GuessingGame.Validators
{
    public class GuessValidator : AbstractValidator<GuessModel>
    {
        public GuessValidator()
        {
            RuleFor(p => p.FirstDigit)
                .NotEmpty()
                .WithMessage("Provide a value for the first digit, please!");

            RuleFor(p => p.FirstDigit)
                .MinimumLength(1)
                .MaximumLength(1)
                .WithMessage("Please, enter the first digit!");

            RuleFor(p => p.FirstDigit)
                .Matches("^[1-9]$")
                .WithMessage("Use 1 - 9 for the first digit!");

            RuleFor(p => p.FirstDigit)
                .Must((model, prop) =>
                {
                    if (prop == model.SecondDigit
                        || prop == model.ThirdDigit
                        || prop == model.FourthDigit)
                        return false;

                    return true;
                })
                .WithMessage("First digit must be unique!");

            RuleFor(p => p.SecondDigit)
                .NotEmpty()
                .WithMessage("Provide a value for the second digit, please!");

            RuleFor(p => p.SecondDigit)
                .MinimumLength(1)
                .MaximumLength(1)
                .WithMessage("Please, enter the second digit!");

            RuleFor(p => p.SecondDigit)
                .Matches("^[0-9]$")
                .WithMessage("Use 0 - 9 for the second digit!");

            RuleFor(p => p.SecondDigit)
                .Must((model, prop) =>
                {
                    if (prop == model.FirstDigit
                        || prop == model.ThirdDigit
                        || prop == model.FourthDigit)
                        return false;

                    return true;
                })
                .WithMessage("Second digit must be unique!");
                
            
            RuleFor(p => p.ThirdDigit)
                .NotEmpty()
                .WithMessage("Provide a value for the third digit, please!");

            RuleFor(p => p.ThirdDigit)
                .MinimumLength(1)
                .MaximumLength(1)
                .WithMessage("Please, enter the third digit!");

            RuleFor(p => p.ThirdDigit)
                .Matches("^[0-9]$")
                .WithMessage("Use 0 - 9 for the third digit!");

            RuleFor(p => p.ThirdDigit)
                .Must((model, prop) =>
                {
                    if (prop == model.SecondDigit
                        || prop == model.FirstDigit
                        || prop == model.FourthDigit)
                        return false;

                    return true;
                })
                .WithMessage("Third digit must be unique!");

            RuleFor(p => p.FourthDigit)
                .NotEmpty()
                .WithMessage("Provide a value for the fourth digit, please!");

            RuleFor(p => p.FourthDigit)
                .MinimumLength(1)
                .MaximumLength(1)
                .WithMessage("Please, enter the fourth digit!");

            RuleFor(p => p.FourthDigit)
                .Matches("^[0-9]$")
                .WithMessage("Use 0 - 9 for the fourth digit!");

            RuleFor(p => p.FourthDigit)
                .Must((model, prop) =>
                {
                    if (prop == model.SecondDigit
                        || prop == model.ThirdDigit
                        || prop == model.FirstDigit)
                        return false;

                    return true;
                })
                .WithMessage("Forth digit must be unique!");
        }
    }
}

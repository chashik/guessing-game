using FluentValidation;
using GuessingGame.Models;
using System.Collections.Generic;

namespace GuessingGame.Validators
{
    public class GameValidator : AbstractValidator<GameModel>
    {
        public GameValidator()
        {
            RuleFor(p => p.CurrentGuess).SetValidator(new GuessValidator());

            RuleFor(p => p.CurrentGuess).Must(
                (gameModel, guessModel) =>
                {
                    IReadOnlyList<GuessModel> guessLog = gameModel.GuessLog;

                    if (guessLog == default(IReadOnlyList<GuessModel>)
                        || gameModel.GuessLog.Count == default)
                        return true;

                    for (int i = 0, l = guessLog.Count; i < l; i++)
                    {
                        GuessModel logEntry = guessLog[i];

                        bool isDoubled =
                            logEntry.FirstDigit == guessModel.FirstDigit
                            && logEntry.SecondDigit == guessModel.SecondDigit
                            && logEntry.ThirdDigit == guessModel.ThirdDigit
                            && logEntry.FourthDigit == guessModel.FourthDigit;

                        if (isDoubled)
                            return false;
                    }

                    return true;
                })
                .WithMessage(
                    (gameModel, guessModel) =>
                    {
                        IReadOnlyList<GuessModel> guessLog = gameModel.GuessLog;

                        for (int i = 0, l = guessLog.Count; i < l; i++)
                        {
                            GuessModel logEntry = guessLog[i];

                            bool isDoubled =
                                logEntry.FirstDigit == guessModel.FirstDigit
                                && logEntry.SecondDigit == guessModel.SecondDigit
                                && logEntry.ThirdDigit == guessModel.ThirdDigit
                                && logEntry.FourthDigit == guessModel.FourthDigit;

                            if (isDoubled)
                                return $"Do not repeat guesses! See Guess Log #{i + 1}.";
                        }

                        return string.Empty;
                    });
        }
    }
}

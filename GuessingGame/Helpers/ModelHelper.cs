using GuessingGame.Domain;
using GuessingGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuessingGame.Helpers
{
    public static class ModelHelper
    {
        public static GuessModel CreateGuessModel(Guess guess, GuessScore guessScore)
        {
            if (guess == default(Guess))
                throw new ArgumentNullException(nameof(guess));

            if (guessScore == default(GuessScore))
                throw new ArgumentNullException(nameof(guessScore));

            if (guess.Id != guessScore.GuessId)
                throw new InvalidOperationException(
                    $"Guess and score mismatch! Guess id: {guess.Id}. " +
                    $"Score.GuessId: {guessScore.GuessId}.");


            return new GuessModel
            {
                FirstDigit = guess.FirstDigit.ToString(),
                SecondDigit = guess.SecondDigit.ToString(),
                ThirdDigit = guess.ThirdDigit.ToString(),
                FourthDigit = guess.FourthDigit.ToString(),
                MatchesWithPosition = guessScore.MatchesWithPosition,
                MatchesWithoutPosition = guessScore.MatchesWithoutPosition
            };
        }

        public static LeaderModel CreateLeaderModel(
            Player player,
            ICollection<Game> games)
        {
            if (player == default(Player))
                throw new ArgumentNullException(nameof(player));

            if (games == default(ICollection<Game>))
                throw new ArgumentNullException(nameof(games));

            return new LeaderModel
            {
                Name = player.Name,
                Score = player.Score,
                GamesPlayed = player.GamesPlayed,
                GuessesMade = player.GuessesMade,
                MatchesWithPosition = games.Sum(p => p.MatchesWithPosition),
                MatchesWithoutPosition = games.Sum(p => p.MatchesWithoutPosition)
            };
        }

        public static LeaderModel[] MergeLeaderModels(LeaderModel[] leaderModels)
        {
            if (leaderModels == default(LeaderModel[]))
                throw new ArgumentNullException(nameof(leaderModels));

            if (leaderModels.Length == 0)
                return leaderModels;

            return leaderModels
                 .GroupBy(p => new
                 {
                     p.Score,
                     p.GamesPlayed,
                     p.GuessesMade
                 })
                 .Select(p => new LeaderModel
                 {
                     Name = string.Join(", ", p.Select(m => m.Name)),
                     GamesPlayed = p.Key.GamesPlayed,
                     GuessesMade = p.Key.GuessesMade,
                     MatchesWithoutPosition = p.First().MatchesWithoutPosition,
                     MatchesWithPosition = p.First().MatchesWithPosition,
                     Score = p.Key.Score
                 })
                 .ToArray();
        }
    }
}

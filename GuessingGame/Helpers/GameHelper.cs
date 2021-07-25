using GuessingGame.Domain;
using GuessingGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuessingGame.Helpers
{
    public static class GameHelper
    {
        public static string CreateSecret()
        {
            Random pseudoRandom = new();

            List<int> source = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            int[] result = new int[4];

            int resultIndex = 1;

            int sourceIndex = pseudoRandom.Next(1, 9);

            result[0] = source[sourceIndex];

            source.RemoveAt(sourceIndex);

            for (int i = 8; i > 5; i--)
            {
                sourceIndex = pseudoRandom.Next(0, i);

                result[resultIndex] = source[sourceIndex];

                resultIndex++;

                source.RemoveAt(sourceIndex);
            }

            return string.Join(string.Empty, result);
        }

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

        public static GuessScore GetGuessScore(Guess guess, string secret)
        {
            if (guess == default(Guess))
                throw new ArgumentNullException(nameof(guess));

            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret));

            Regex regEx = new("^[0-9]{4}$");

            if (!regEx.IsMatch(secret))
                throw new ArgumentException($"Invalid secret: {secret}!", nameof(secret));

            if (secret.Distinct().Count() != 4)
                throw new ArgumentException($"Invalid secret: {secret}!", nameof(secret));

            int[] digits = new int[]
            {
                guess.FirstDigit,
                guess.SecondDigit,
                guess.ThirdDigit,
                guess.FourthDigit
            };

            if (digits.Any(p => p > 9))
                throw new InvalidOperationException(
                    $"Invalid data state! Not a digit [0-9] found: {string.Join(", ", digits)}!");

            if (digits.Distinct().Count() < 4)
                throw new InvalidOperationException(
                    $"Invalid data state! Guess with id {guess.Id} " +
                    $"contains non-unique elements: {string.Join(", ", digits)}!");

            double score = .0;

            int matchesWithPosition = 0;

            int matchesWithoutPosition = 0;

            for (int i = 0; i < 4; i++)
            {
                string digit = digits[i].ToString();

                if (secret.Substring(i, 1) == digit)
                {
                    score += 1;

                    matchesWithPosition += 1;
                }
                else if (secret.IndexOf(digit) > -1)
                {
                    score += 0.5;

                    matchesWithoutPosition += 1;
                }
            }

            return new GuessScore
            {
                GuessId = guess.Id,
                Score = score,
                MatchesWithoutPosition = matchesWithoutPosition,
                MatchesWithPosition = matchesWithPosition
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

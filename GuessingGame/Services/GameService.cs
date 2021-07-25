using GuessingGame.Data;
using GuessingGame.Domain;
using GuessingGame.Helpers;
using GuessingGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Guess> _guessRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IPlayerService _playerService;

        public GameService(
            IRepository<Guess> guessRepository,
            IRepository<Game> gameRepository,
            IPlayerService playerService
            )
        {
            _guessRepository = guessRepository;
            _gameRepository = gameRepository;
            _playerService = playerService;
        }

        public async Task<bool> IsGameOver(GameModel model)
        {
            if (model == default(GameModel))
                throw new ArgumentNullException(nameof(model));

            Player player = await _playerService.GetPlayerByNameAsync(model.PlayerName);

            if (player == default(Player))
                throw new ArgumentException("Invalid PlayerName!", nameof(model));

            Game game = _gameRepository.Table // we restore unfinished games
                .FirstOrDefault(p => p.PlayerId == player.Id && !p.IsOver);

            #region If Game Is New

            if (game == default(Game))
            {
                model.CurrentGuess = model.PreviousTry = model.BestResult = default;

                model.GuessLog = default;

                game = new Game
                {
                    PlayerId = player.Id,
                    Secret = GameHelper.CreateSecret(),
                    IsOver = false
                };

                await _gameRepository.Insert(game);
            }

            #endregion

            string secret = game.Secret;

            if (string.IsNullOrEmpty(secret) || secret.Length != 4)
                throw new InvalidOperationException(
                    "Invalid state: secret is empty or its length does not equal 4!");

            #region If Guess Is Made

            if (model.CurrentGuess != default(GuessModel))
            {
                GuessModel currentGuess = model.CurrentGuess;

                Guess guess = new() { GameId = game.Id, };

                if (int.TryParse(currentGuess.FirstDigit, out int parseResult) && parseResult < 10)
                    guess.FirstDigit = parseResult;
                else
                    throw new ArgumentException(
                        $"Can not parse first digit or number more than 9: {currentGuess.FirstDigit}!",
                        nameof(model));

                if (int.TryParse(currentGuess.SecondDigit, out parseResult) && parseResult < 10)
                    guess.SecondDigit = parseResult;
                else
                    throw new ArgumentException(
                        $"Can not parse second digit or number more than 9: {currentGuess.SecondDigit}!", 
                        nameof(model));

                if (int.TryParse(currentGuess.ThirdDigit, out parseResult) && parseResult < 10)
                    guess.ThirdDigit = parseResult;
                else
                    throw new ArgumentException(
                        $"Can not parse third digit or number more than 9: {currentGuess.ThirdDigit}!", 
                        nameof(model));

                if (int.TryParse(currentGuess.FourthDigit, out parseResult) && parseResult < 10)
                    guess.FourthDigit = parseResult;
                else
                    throw new ArgumentException(
                        $"Can not parse fourth digit or number more than 9: {currentGuess.FourthDigit}!", 
                        nameof(model));

                await _guessRepository.Insert(guess);

                model.CurrentGuess = default;
            }

            #endregion

            #region Populating Game Model

            Guess[] guesses = _guessRepository.Table
                .Where(p => p.GameId == game.Id)
                .OrderBy(p => p.Id)
                .ToArray();

            int guessesLength = guesses.Length;

            model.TriesLeft = 8 - guessesLength;

            if (guessesLength == 0)
                return false;

            GuessScore[] guessScores = guesses
                .Select(p => GameHelper.GetGuessScore(p, secret))
                .OrderBy(p => p.Score)
                .ThenBy(p => p.GuessId)
                .ToArray();

            Guess previousGuess = guesses.Last();

            GuessScore previousScore = guessScores.First(s => s.GuessId == previousGuess.Id);

            model.PreviousTry = GameHelper.CreateGuessModel(previousGuess, previousScore);

            GuessScore bestScore = guessScores.Last();

            Guess bestGuess = guesses.First(p => p.Id == bestScore.GuessId);

            model.BestResult = GameHelper.CreateGuessModel(bestGuess, bestScore);

            model.GuessLog = guesses
                .Select(p => GameHelper.CreateGuessModel(
                    p,
                    guessScores.First(s => s.GuessId == p.Id)))
                .ToArray();

            #endregion

            #region If Game Is Over

            if (!game.IsOver
                && (bestScore.Score == 4 || guessesLength > 7))
            {
                player.GuessScore += bestScore.Score;

                player.GuessesMade += guessesLength;

                player.GamesPlayed += 1;

                await _playerService.UpdatePlayer(player);

                game.MatchesWithPosition = bestScore.MatchesWithPosition;

                game.MatchesWithoutPosition = bestScore.MatchesWithoutPosition;
                
                game.IsOver = true;

                await _gameRepository.Update(game);

                model.Secret = secret;

                model.Score = bestScore.Score;

                return true;
            }

            #endregion

            return false;
        }

        public async Task<ICollection<Game>> GetGamesByPlayerId(int playerId) =>
            await Task.FromResult(_gameRepository.Table
                .Where(p => p.PlayerId == playerId && p.IsOver)
                .ToArray());
    }
}


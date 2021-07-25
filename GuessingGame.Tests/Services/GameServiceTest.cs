using GuessingGame.Data;
using GuessingGame.Domain;
using GuessingGame.Models;
using GuessingGame.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuessingGame.Tests.Services
{
    public class GameServiceTest
    {
        private GameService _gameService;

        private List<Guess> _guesses;

        private List<Game> _games;

        private List<Player> _players;

        private GameModel _gameModel;

        [SetUp]
        public void Setup()
        {
            _guesses = new()
            {
                new Guess
                {
                    Id = 1,
                    GameId = 1,
                    FirstDigit = 5,
                    SecondDigit = 7,
                    ThirdDigit = 9,
                    FourthDigit = 4
                },
                new Guess
                {
                    Id = 2,
                    GameId = 1,
                    FirstDigit = 2,
                    SecondDigit = 0,
                    ThirdDigit = 9,
                    FourthDigit = 4
                },
                new Guess
                {
                    Id = 3,
                    GameId = 1,
                    FirstDigit = 3,
                    SecondDigit = 0,
                    ThirdDigit = 8,
                    FourthDigit = 4
                },
                new Guess
                {
                    Id = 4,
                    GameId = 1,
                    FirstDigit = 4,
                    SecondDigit = 0,
                    ThirdDigit = 1,
                    FourthDigit = 6
                },
            };

            _games = new()
            {
                new Game
                {
                    Id = 1,
                    BestGuessId = 4,
                    IsOver = true,
                    MatchesWithPosition = 4,
                    MatchesWithoutPosition = 0,
                    PlayerId = 1,
                    Secret = "4016"
                }
            };

            _players = new()
            {
                new Player
                {
                    Id = 1,
                    GamesPlayed = 1,
                    GuessesMade = 4,
                    GuessScore = 4,
                    Name = "Jack"
                }
            };

            _gameModel = new();

            Mock<IRepository<Guess>> guessRepositoryMock = new();

            guessRepositoryMock.Setup(p => p.Table).Returns(_guesses.AsQueryable());

            Mock<IRepository<Game>> gameRepositoryMock = new();

            gameRepositoryMock.Setup(p => p.Table).Returns(_games.AsQueryable());

            Mock<IPlayerService> playerServiceMock = new();

            playerServiceMock.Setup(p => p.GetPlayerByNameAsync(_players[0].Name)).ReturnsAsync(_players[0]);

            _gameService = new GameService(
                guessRepositoryMock.Object,
                gameRepositoryMock.Object,
                playerServiceMock.Object);
        }

        [Test]
        public void ShouldThrowArgumentExceptionForEmptyGameModel()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _gameService.IsGameOver(_gameModel));
        }

        [Test]
        public void ShouldThrowArgumentExceptionForRepeatingDigitsInSecret()
        {
            _games[0].IsOver = false;

            _games[0].Secret = "2235";

            _gameModel.PlayerName = _players[0].Name;

            Assert.ThrowsAsync<ArgumentException>(async () => await _gameService.IsGameOver(_gameModel));
        }
    }
}

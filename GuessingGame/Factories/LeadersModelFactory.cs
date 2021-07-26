using GuessingGame.Helpers;
using GuessingGame.Models;
using GuessingGame.Services;
using System;
using System.Threading.Tasks;

namespace GuessingGame.Factories
{
    public class LeadersModelFactory : ILeadersModelFactory
    {
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;

        public LeadersModelFactory(
            IPlayerService playerService,
            IGameService gameService
            )
        {
            _playerService = playerService;
            _gameService = gameService;
        }

        public async Task<LeadersModel> GetLeadersModel(string playerName, int topCount)
        {
            if (string.IsNullOrEmpty(playerName))
                throw new ArgumentNullException(nameof(playerName));

            if (topCount == default)
                throw new ArgumentNullException(nameof(topCount));

            System.Collections.Generic.IReadOnlyList<Domain.Player> leaders = await _playerService.GetLeaders(topCount);

            int leadersCount = leaders.Count;

            LeaderModel[] leaderModels = new LeaderModel[leadersCount];

            if (leadersCount > 0)
                for (int i = 0; i < leadersCount; i++)
                {
                    Domain.Player leader = leaders[i];

                    System.Collections.Generic.ICollection<Domain.Game> games = await _gameService.GetGamesByPlayerId(leader.Id);

                    leaderModels[i] = ModelHelper.CreateLeaderModel(leader, games);
                }

            return new LeadersModel
            {
                IsAuthorized = true,
                PlayerName = playerName,
                Leaders = ModelHelper.MergeLeaderModels(leaderModels)
            };
        }

        
    }
}

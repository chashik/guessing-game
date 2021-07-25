using GuessingGame.Data;
using GuessingGame.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<Player> _playerRepository;

        public PlayerService(IRepository<Player> playerRepository) => 
            _playerRepository = playerRepository;

        public async Task<Player> GetPlayerByNameAsync(string name) =>
            await Task.FromResult(_playerRepository.Table.FirstOrDefault(p => p.Name == name));

        public async Task AddPlayerAsync(Player player) => 
            await _playerRepository.Insert(player);

        public async Task UpdatePlayer(Player player) =>
            await _playerRepository.Update(player);

        public async Task<IReadOnlyList<Player>> GetLeaders(int count)
        {
            return await Task.FromResult(_playerRepository.Table
                .Where(p => p.GamesPlayed > 0)
                .OrderByDescending(p => p.GamesPlayed == 0 ? .0 : p.GuessScore / p.GamesPlayed)
                .ThenBy(p => p.GuessesMade)
                .Take(count)
                .ToArray());
        }
    }
}

using GuessingGame.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByNameAsync(string name);
        
        Task AddPlayerAsync(Player player);

        Task UpdatePlayer(Player player);
        Task<IReadOnlyList<Player>> GetLeaders(int count);
    }
}
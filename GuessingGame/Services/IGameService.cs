using GuessingGame.Domain;
using GuessingGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Makes gues and determenes whether a game is over.
        /// </summary>
        /// <param name="gameModel">GameModel instance</param>
        /// <returns>True if game is over, False otherwise.</returns>
        Task<bool> IsGameOver(GameModel model);

        Task<ICollection<Game>> GetGamesByPlayerId(int playerId);
    }
}

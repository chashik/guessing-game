using GuessingGame.Domain;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SignInAsync(Player player, bool isPersistent);

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SignOutAsync();

        /// <summary>
        /// Get authenticated player
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the player
        /// </returns>
        Task<Player> GetAuthenticatedPlayerAsync();
    }
}

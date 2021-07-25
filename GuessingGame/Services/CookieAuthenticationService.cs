using GuessingGame.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GuessingGame.Services
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly IPlayerService _playerService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Player _cachedPlayer;

        #endregion

        #region Ctor

        public CookieAuthenticationService(
            IPlayerService playerService,
            IHttpContextAccessor httpContextAccessor)
        {
            _playerService = playerService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SignInAsync(Player player, bool isPersistent)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            //create claims for player's username and email
            List<Claim> claims = new();

            if (!string.IsNullOrEmpty(player.Name))
                claims.Add(new Claim(
                    ClaimTypes.Name, 
                    player.Name, 
                    ClaimValueTypes.String, 
                    AuthenticationDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            ClaimsIdentity userIdentity = new(
                claims, 
                AuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal userPrincipal = new(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            AuthenticationProperties authenticationProperties = new()
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(
                AuthenticationDefaults.AuthenticationScheme, 
                userPrincipal, 
                authenticationProperties);

            //cache authenticated player
            _cachedPlayer = player;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SignOutAsync()
        {
            //reset cached player
            _cachedPlayer = default;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(
                AuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated player
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the player
        /// </returns>
        public virtual async Task<Player> GetAuthenticatedPlayerAsync()
        {
            //whether there is a cached player
            if (_cachedPlayer != default(Player))
                return _cachedPlayer;

            //try to get authenticated user identity
            AuthenticateResult authenticateResult = await _httpContextAccessor.HttpContext?.AuthenticateAsync(
                AuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return default;

            Player player = default;

            //try to get player by username
            Claim usernameClaim = authenticateResult.Principal
                .FindFirst(claim => claim.Type == ClaimTypes.Name
                    && claim.Issuer.Equals(
                        AuthenticationDefaults.ClaimsIssuer,
                        StringComparison.InvariantCultureIgnoreCase));

            if (usernameClaim != default(Claim))
                player = await _playerService.GetPlayerByNameAsync(usernameClaim.Value);

            if (player == default(Player)/*|| !await _playerService.IsRegisteredAsync(player)*/)
                return default;

            //cache authenticated player
            _cachedPlayer = player;

            return _cachedPlayer;
        }

        #endregion
    }
}

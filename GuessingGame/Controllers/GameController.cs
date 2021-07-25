using GuessingGame.Domain;
using GuessingGame.Models;
using GuessingGame.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GuessingGame.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGameService _gameService;

        public GameController(
            ILogger<HomeController> logger,
            IAuthenticationService authenticationService,
            IGameService gameService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Player player = await _authenticationService.GetAuthenticatedPlayerAsync();

            if (player == default)
                return await Task.FromResult<IActionResult>(
                    new RedirectToActionResult("Login", "Home", new { }));

            return await Index( // attempt to resume unfinished game if requested via GET
                new GameModel 
                { 
                    PlayerName = player.Name 
                }); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(GameModel model)
        {
            Player player = await _authenticationService.GetAuthenticatedPlayerAsync();

            if (player == default(Player))
                return await Task.FromResult<IActionResult>(
                    new StatusCodeResult((int)HttpStatusCode.Unauthorized));

            if (!ModelState.IsValid)
                return await Task.FromResult<IActionResult>(View(model));

            if (player.Name != model.PlayerName)
                throw new InvalidOperationException(
                    "Invalid state: common authentication and game player name mismatch!");

            if (await _gameService.IsGameOver(model))
                return await Task.FromResult<IActionResult>(View("Over", model));

            // ModelState need to be cleared to keep data up-to-date
            // between server-side validation postbacks with errors
            ModelState.Clear(); 

            return await Task.FromResult<IActionResult>(View(model));
        }
    }
}

using GuessingGame.Domain;
using GuessingGame.Factories;
using GuessingGame.Models;
using GuessingGame.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace GuessingGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlayerService _playerService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILeadersModelFactory _leadersModelFactory;

        public HomeController(
            ILogger<HomeController> logger,
            IPlayerService playerService,
            IAuthenticationService authenticationService,
            ILeadersModelFactory leadersModelFactory)
        {
            _logger = logger;
            _playerService = playerService;
            _authenticationService = authenticationService;
            _leadersModelFactory = leadersModelFactory;
        }

        
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            LoginModel loginModel = new();

            Player player = await _authenticationService.GetAuthenticatedPlayerAsync();

            if (player == default(Player))
                return await Task.FromResult<IActionResult>(View(loginModel));
            else
                return await Task.FromResult<IActionResult>(RedirectToAction("Index", "Game"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Player player = await _playerService.GetPlayerByNameAsync(model.PlayerName);

                if (player == default(Player))
                {
                    player = new Player { Name = model.PlayerName };

                    await _playerService.AddPlayerAsync(player);

                }

                await _authenticationService.SignInAsync(player, true);

                return RedirectToAction("Index", "Game");
            }

            return await Task.FromResult<IActionResult>(View(model));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Player player = await _authenticationService.GetAuthenticatedPlayerAsync();

            if (player != default(Player))
                await _authenticationService.SignOutAsync();

            return await Task.FromResult<IActionResult>(
                new RedirectToActionResult("Login", "Home", new { }));
        }

        [HttpGet]
        public async Task<IActionResult> Leaders()
        {
            Player player = await _authenticationService.GetAuthenticatedPlayerAsync();

            if (player == default(Player)) // Let's hide leaders from unauthorized vistors
                return await Task.FromResult<IActionResult>(
                    new StatusCodeResult((int)HttpStatusCode.Unauthorized));

            LeadersModel model = await _leadersModelFactory.GetLeadersModel(player.Name, 20);

            return await Task.FromResult<IActionResult>(View(model));
        }

        public async Task<IActionResult> Privacy() => 
            await Task.FromResult<IActionResult>(View());

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error() => 
            await Task.FromResult<IActionResult>(View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }));
    }
}

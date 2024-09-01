using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.SearchParams;
using PL.MVC.Infrastructure.Models;
using Entities;
using Common.ConvertParams;
using BL.Interfaces.InGame;

namespace PL.MVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IUsersBL _usersBL;
        private readonly ISessionsBL _sessionsBL;
        private readonly IGamesBL _gamesBL;
        private readonly IGameManagerBL _gameManagerBL;

        public GameController(
            IUsersBL usersBL,
            ISessionsBL sessionsBL,
            IGamesBL gamesBL,
            IGameManagerBL gameManagerBL)
        {
            _usersBL = usersBL;
            _sessionsBL = sessionsBL;
            _gamesBL = gamesBL;
            _gameManagerBL = gameManagerBL;
        }

        public async Task<IActionResult> Index()
        {
            var player = UserModel.FromEntity((await _usersBL.GetAsync(
                new UsersSearchParams()
                {
                    Login = User.Identity!.Name
                },
                new UsersConvertParams()
                {
                    IsIncludeSessions = true
                })).Objects.FirstOrDefault()!);

            if (player.Sessions.Count == 0)
            {
                var games = (await _gamesBL.GetAsync(
                    new GamesSearchParams()
                    {
                        IsActive = true
                    },
                    new GamesConvertParams()
                    {
                        IsIncludeSessions = true
                    })).Objects;

                var game = games.FirstOrDefault(game => game.Sessions.Count < 3);
                if (games.Count == 0 || game == null)
                {
                    game = new Game(
                        0,
                        "Ladder game",
                        DateTime.Now,
                        null,
                        null,
                        null,
                        true
                        );

                    var gameId = await _gamesBL.AddOrUpdateAsync(game);
                    game.Id = gameId;
                }

                var session = new Session(
                    0,
                    UserModel.ToEntity(player),
                    game!,
                    true
                    );

                await _sessionsBL.AddOrUpdateAsync(session);

                _gameManagerBL.InitializeGame(game);
            }

            return View();
        }
    }
}

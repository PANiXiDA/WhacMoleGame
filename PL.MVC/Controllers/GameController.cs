using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.SearchParams;
using PL.MVC.Infrastructure.Models;
using Entities;
using Common.ConvertParams;
using InGame.IManagers;
using InGame.Models.Requests;
using System.Numerics;

namespace PL.MVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IUsersBL _usersBL;
        private readonly ISessionsBL _sessionsBL;
        private readonly IGamesBL _gamesBL;
        private readonly IGameManager _gameManager;

        public GameController(
            IUsersBL usersBL,
            ISessionsBL sessionsBL,
            IGamesBL gamesBL,
            IGameManager gameManager)
        {
            _usersBL = usersBL;
            _sessionsBL = sessionsBL;
            _gamesBL = gamesBL;
            _gameManager = gameManager;
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

                var game = games.FirstOrDefault(game => game.Sessions?.Count < 3);
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

                _gameManager.InitializeGame(game);
            }
            else
            {
                var game = player.Sessions.First().Game;
                var sessions = SessionModel.FromEntitiesList((await _sessionsBL.GetAsync(new SessionsSearchParams()
                {
                    GameId = game.Id
                })).Objects);
                game.Sessions = sessions;

                _gameManager.InitializeGame(GameModel.ToEntity(game));
            }

            return View(player);
        }

        public async Task<IActionResult> CreateGame()
        {
            var player = UserModel.FromEntity((await _usersBL.GetAsync(
                new UsersSearchParams()
                {
                    Login = User.Identity!.Name
                })).Objects.FirstOrDefault()!);

            var game = new Game(
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

            var session = new Session(
                0,
                UserModel.ToEntity(player),
                game!,
                true
                );

            await _sessionsBL.AddOrUpdateAsync(session);

            _gameManager.InitializeGame(game);

            return View("Index", player);
        }

        public async Task<IActionResult> JoinGame(int gameId)
        {
            var player = UserModel.FromEntity((await _usersBL.GetAsync(
                new UsersSearchParams()
                {
                    Login = User.Identity!.Name
                })).Objects.FirstOrDefault()!);

            var game = (await _gamesBL.GetAsync(
                gameId,
                new GamesConvertParams()
                {
                    IsIncludeSessions = true
                }));

            if (!game.Sessions!.Any(session => session.Player.Id == player.Id))
            {
                var session = new Session(
                    0,
                    UserModel.ToEntity(player),
                    game!,
                    true
                    );

                await _sessionsBL.AddOrUpdateAsync(session);
            }

            _gameManager.InitializeGame(game);

            return View("Index", player);
        }


        [HttpGet]
        public IActionResult GetGameState()
        {
            var gameState = _gameManager.GetGameState();

            return Ok(gameState);
        }

        [HttpPost]
        public IActionResult PlayerMove([FromBody] PlayerMoveRequest request)
        {
            _gameManager.PlayerMove(request.PlayerLogin, request.TileId);
            var gameState = _gameManager.GetGameState();

            return Ok(gameState);
        }
    }
}

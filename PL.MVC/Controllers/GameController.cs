using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.SearchParams;
using PL.MVC.Infrastructure.Models;
using Entities;
using Common.ConvertParams;
using InGame.IManagers;
using InGame.Models.Requests;
using Common.Enums;
using PL.MVC.Infrastructure.ViewModels;

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

            GameModel? game = null;

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

                if (games.Count == 0)
                {
                    game = new GameModel
                    {
                        Id = 0,
                        Name = "Ladder game",
                        GameStartTime = DateTime.Now,
                        GameEndTime = null,
                        Winner = null,
                        MaxPointsCount = null,
                        IsActive = true
                    };

                    var gameId = await _gamesBL.AddOrUpdateAsync(GameModel.ToEntity(game));
                    game.Id = gameId;
                }
                else
                {
                    game = GameModel.FromEntity(games.First(game => game.Sessions?.Count < 3));
                }

                var session = new Session(
                    0,
                    UserModel.ToEntity(player),
                    GameModel.ToEntity(game),
                    true
                    );

                await _sessionsBL.AddOrUpdateAsync(session);
                game.Sessions = new List<SessionModel>()
                {
                    SessionModel.FromEntity(session)
                };

                _gameManager.InitializeGame(GameModel.ToEntity(game));
            }
            else
            {
                game = player.Sessions.First().Game;
                var sessions = SessionModel.FromEntitiesList((await _sessionsBL.GetAsync(new SessionsSearchParams()
                {
                    GameId = game.Id
                })).Objects);
                game.Sessions = sessions;

                _gameManager.InitializeGame(GameModel.ToEntity(game));
            }
            var model = new GameViewModel()
            {
                Player = player,
                GameId = game.Id
            };

            return View(model);
        }

        public async Task<IActionResult> CreateGame(string gameName)
        {
            var player = UserModel.FromEntity((await _usersBL.GetAsync(
                new UsersSearchParams()
                {
                    Login = User.Identity!.Name
                })).Objects.FirstOrDefault()!);

            var game = new Game(
                0,
                gameName,
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

            session.Id = await _sessionsBL.AddOrUpdateAsync(session);
            game.Sessions = new List<Session> { session };

            _gameManager.InitializeGame(game);

            var model = new GameViewModel()
            {
                Player = player,
                GameId = gameId
            };

            return View("Index", model);
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

            var model = new GameViewModel()
            {
                Player = player,
                GameId = gameId
            };

            return View("Index", model);
        }


        [HttpGet]
        public IActionResult GetGameState([FromQuery] int gameId)
        {
            var gameState = _gameManager.GetGameState(gameId);

            return Ok(gameState);
        }

        [HttpPost]
        public IActionResult PlayerMove([FromBody] PlayerMoveRequest request)
        {
            _gameManager.PlayerMove(request.PlayerLogin, request.TileId, request.GameId);
            var gameState = _gameManager.GetGameState(request.GameId);

            return Ok(gameState);
        }

        [HttpPost]
        public async Task GameOver([FromQuery] int gameId)
        {
            var gameSession = _gameManager.GameSessions.First(item => item.Game.Id == gameId);

            string playerWithMaxScore = gameSession.PlayerScores
                .OrderByDescending(x => x.Value).FirstOrDefault().Key;
            int maxScore = gameSession.PlayerScores.Max(x => x.Value);

            var winner = await _usersBL.GetAsync(playerWithMaxScore);

            var game = await _gamesBL.GetAsync(gameId);
            game.Winner = winner;
            game.MaxPointsCount = maxScore;
            game.GameEndTime = DateTime.Now;
            game.IsActive = false;

            await _gamesBL.AddOrUpdateAsync(game);

            var sessions = (await _sessionsBL.GetAsync(new SessionsSearchParams()
            {
                GameId = game.Id
            })).Objects;

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _sessionsBL.AddOrUpdateAsync(sessions);

            _gameManager.RemoveGameSession(gameId);
        }

        [HttpGet]
        public async Task<JsonResult> GetGames(GamesFilter filter, int page = 1, int pageSize = 6)
        {
            var userIdClaim = User.FindFirst("UserId");
            int? userId = null;
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }

            List<GameModel> games = new List<GameModel>();
            switch (filter)
            {
                case GamesFilter.All:
                    games = GameModel.FromEntitiesList((await _gamesBL.GetAsync(new GamesSearchParams() { })).Objects);
                    break;
                case GamesFilter.My:
                    games = GameModel.FromEntitiesList((await _gamesBL.GetAsync(new GamesSearchParams() { WinnerId = userId })).Objects);
                    break;
                case GamesFilter.Active:
                    games = GameModel.FromEntitiesList((await _gamesBL.GetAsync(new GamesSearchParams() { IsActive = true })).Objects);
                    break;
                case GamesFilter.Finished:
                    games = GameModel.FromEntitiesList((await _gamesBL.GetAsync(new GamesSearchParams() { IsActive = false })).Objects);
                    break;
                default:
                    break;
            }

            var totalItems = games.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var pagedGames = games.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                Games = pagedGames,
                TotalPages = totalPages
            };

            return Json(result);
        }
    }
}

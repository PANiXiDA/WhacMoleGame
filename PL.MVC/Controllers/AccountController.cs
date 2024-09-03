using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PL.MVC.Infrastructure.Claims;
using PL.MVC.Infrastructure.Responses;
using PL.MVC.Infrastructure.ViewModel;
using System.Security.Claims;
using BL.Interfaces;
using Common.SearchParams;
using Entities;
using Common.Enums;
using PL.MVC.Infrastructure.Models;
using PL.MVC.Infrastructure.ViewModels;
using PL.MVC.Infrastructure.Requests;
using Microsoft.AspNetCore.Authorization;
using Common;
using Common.ConvertParams;

namespace PL.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersBL _userBL;
        private readonly IGamesBL _gamesBL;

        public AccountController(IUsersBL userBL, IGamesBL gamesBL)
        {
            _userBL = userBL;
            _gamesBL = gamesBL;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Public" });
            }

            var user = UserModel.FromEntity((await _userBL.GetAsync(
                new UsersSearchParams()
                {
                    Login = User.Identity.Name
                },
                new UsersConvertParams()
                {
                    IsIncludeAllSessions = true
                })).Objects.First());

            int countGames = user.Sessions?.Count() ?? 0;
            int countWins = user.Sessions?.Count(s => s.Game?.Winner?.Id == user.Id) ?? 0;
            int maxPointsCount = user.Sessions?
                .Where(s => s.Game?.Winner?.Id == user.Id && s.Game?.MaxPointsCount != null)
                .Max(s => s.Game!.MaxPointsCount ?? 0) ?? 0;

            var model = new AccountViewModel()
            {
                User = user,
                CountGames = countGames,
                CountWins = countWins,
                MaxPointsCount = maxPointsCount
            };

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginViewModel model)
        {
            var response = new BaseResponse();
            var user = await _userBL.VerifyPasswordAsync(model.Login, model.Password);

            if (user == null)
            {
                response.IsSuccess = false;
                response.TextError = "Incorrect login information is specified!";
                return Json(response);
            }

            if (user.IsBlocked)
            {
                response.IsSuccess = false;
                response.TextError = "The user is blocked!";
                return Json(response);
            }

            var identity = new CustomUserIdentity(user.Id, user.Login, user.Role);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties() { IsPersistent = model.Remember }
            );

            return Json(response);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Registration([FromBody] RegistrationViewModel model)
        {
            var response = new BaseResponse();

            if (model.Password != model.RepeatPassword)
            {
                response.IsSuccess = false;
                response.TextError = "Passwords must match!";
                return Json(response);
            }

            if (!Helpers.IsPasswordValid(model.Password))
            {
                response.IsSuccess = false;
                response.TextError = "Invalid password, it must contain 6 or more characters, 1 uppercase, 1 lowercase and 1 digit!";
                return Json(response);
            }

            var userLogin = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Login = model.Login
            })).Objects;

            if (userLogin.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this username has already been registered!";
                return Json(response);
            }

            var userEmail = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Email = model.Email
            })).Objects;

            if (userEmail.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with such an email has already been registered!";
                return Json(response);
            }

            var userPhoneNumber = (await _userBL.GetAsync(new UsersSearchParams()
            {
                PhoneNumber = model.PhoneNumber
            })).Objects;

            if (userPhoneNumber.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this phone number has already been registered!";
                return Json(response);
            }

            var user = new User(
                0,
                model.Login,
                model.Password,
                model.Email,
                model.PhoneNumber,
                UserRole.Player,
                false,
                DateTime.Now);

            var userId = await _userBL.AddOrUpdateAsync(user);

            var identity = new CustomUserIdentity(userId, user.Login, user.Role);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            return Json(response);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home", new { area = "Public" });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var user = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Login = User.Identity!.Name
            })).Objects.FirstOrDefault()!;

            var response = new BaseResponse();

            if (!string.IsNullOrEmpty(request.Password) && !Helpers.IsPasswordValid(request.Password))
            {
                response.IsSuccess = false;
                response.TextError = "Invalid password, it must contain 6 or more characters, 1 uppercase, 1 lowercase and 1 digit!";
                return Json(response);
            }

            var userLogin = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Login = request.Login
            })).Objects.FirstOrDefault();

            if (userLogin != null && userLogin.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this username has already been registered!";
                return Json(response);
            }

            var userEmail = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Email = request.Email
            })).Objects.FirstOrDefault();

            if (userEmail != null && userEmail.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with such an email has already been registered!";
                return Json(response);
            }

            var userPhoneNumber = (await _userBL.GetAsync(new UsersSearchParams()
            {
                PhoneNumber = request.PhoneNumber
            })).Objects.FirstOrDefault();

            if (userPhoneNumber != null && userPhoneNumber.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this phone number has already been registered!";
                return Json(response);
            }

            user.Login = request.Login;
            user.Password = request.Password;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await _userBL.AddOrUpdateAsync(user);

            return Json(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetGames(GamesFilter filter, int page = 1, int pageSize = 10)
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

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var users = UserModel.FromEntitiesList((await _userBL.GetAsync(new UsersSearchParams())).Objects);

            var totalItems = users.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                Users = pagedUsers,
                TotalPages = totalPages
            };

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Developer)}")]
        public async Task<JsonResult> BlockAndUnblockUser(int id, bool block)
        {
            var user = await _userBL.GetAsync(id);
            user.IsBlocked = block;

            var userId = await _userBL.AddOrUpdateAsync(user);

            return Json(new { ok = true });
        }
    }
}

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
using Tools.SymmetricEncryption.AesEncryption;

namespace PL.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersBL _userBL;
        private readonly IEmailNotificationsBL _emailNotificationsBL;
        private readonly IConfirmationCodesBL _confirmationCodesBL;
        private readonly AesEncryption _encryption;
        private readonly Random _random;

        public AccountController(
            IUsersBL userBL,
            IEmailNotificationsBL emailNotificationsBL,
            IConfirmationCodesBL confirmationCodesBL,
            AesEncryption encryption)
        {
            _userBL = userBL;
            _emailNotificationsBL = emailNotificationsBL;
            _confirmationCodesBL = confirmationCodesBL;
            _encryption = encryption;
            _random = new Random();
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
                    Login = User.Identity.Name,
                    RegistrationStatus = UserRegistrationStatus.Confirmed
                },
                new UsersConvertParams()
                {
                    IsIncludeAllSessions = true
                })).Objects.First());

            int countGames = user.Sessions?.Count() ?? 0;
            int countWins = user.Sessions?.Count(s => s.Game?.Winner?.Id == user.Id) ?? 0;
            int? maxPointsCount = user.Sessions?
                .Where(s => s.Game?.Winner?.Id == user.Id && s.Game?.MaxPointsCount != null)
                .Select(s => s.Game!.MaxPointsCount ?? 0)
                .DefaultIfEmpty(0)
                .Max();

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
                Login = model.Login,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects;

            if (userLogin.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this username has already been registered!";
                return Json(response);
            }

            var userEmail = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Email = model.Email,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects;

            if (userEmail.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with such an email has already been registered!";
                return Json(response);
            }

            var userPhoneNumber = (await _userBL.GetAsync(new UsersSearchParams()
            {
                PhoneNumber = model.PhoneNumber,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects;

            if (userPhoneNumber.Count > 0)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this phone number has already been registered!";
                return Json(response);
            }

            User? user = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Login = model.Login,
                RegistrationStatus = UserRegistrationStatus.Unconfirmed
            })).Objects.FirstOrDefault();

            if (user != null)
            {
                user.Login = model.Login;
                user.Password = model.Password;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.RegistrationDate = DateTime.Now;
            }
            else
            {
                user = new User(
                    0,
                    model.Login,
                    model.Password,
                    model.Email,
                    model.PhoneNumber,
                    UserRole.Player,
                    false,
                    DateTime.Now,
                    UserRegistrationStatus.Unconfirmed);
            }

            var userId = await _userBL.AddOrUpdateAsync(user);

            int code = _random.Next(1000, 10000);

            var entityCode = new ConfirmationCode(
                0,
                code.ToString()
                );

            var confirmationCodeId = await _confirmationCodesBL.AddOrUpdateAsync(entityCode);

            await _emailNotificationsBL.SendCodeConfirmation(model.Email, code);

            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(10),
                HttpOnly = true,
                Secure = true
            };
            Response.Cookies.Append("confirmationCodeId", confirmationCodeId.ToString(), options);
            Response.Cookies.Append("userId", userId.ToString(), options);

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> VerifyCodeConfirmation([FromBody] ConfirmationCodeRequest request)
        {
            var response = new BaseResponse();

            string? cookieCodeId = Request.Cookies["confirmationCodeId"];
            string? cookieUserId = Request.Cookies["userId"];

            if (int.TryParse(cookieCodeId, out int codeId) && int.TryParse(cookieUserId, out int userId))
            {
                var confirmationCode = await _confirmationCodesBL.GetAsync(codeId);

                if (confirmationCode.Code == request.Code)
                {
                    var user = await _userBL.GetAsync(userId);
                    user.RegistrationStatus = UserRegistrationStatus.Confirmed;
                    await _userBL.AddOrUpdateAsync(user);

                    var identity = new CustomUserIdentity(userId, user.Login, user.Role);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity)
                    );

                    return Json(response);
                }
            }

            response.IsSuccess = false;
            response.TextError = "Invalid confirmation code!";
            return Json(response);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var user = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Login = User.Identity!.Name,
                RegistrationStatus = UserRegistrationStatus.Confirmed
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
                Login = request.Login,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects.FirstOrDefault();

            if (userLogin != null && userLogin.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this username has already been registered!";
                return Json(response);
            }

            var userEmail = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Email = request.Email,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects.FirstOrDefault();

            if (userEmail != null && userEmail.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with such an email has already been registered!";
                return Json(response);
            }

            var userPhoneNumber = (await _userBL.GetAsync(new UsersSearchParams()
            {
                PhoneNumber = request.PhoneNumber,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects.FirstOrDefault();

            if (userPhoneNumber != null && userPhoneNumber.Id != user.Id)
            {
                response.IsSuccess = false;
                response.TextError = "A user with this phone number has already been registered!";
                return Json(response);
            }

            if (request.Login != user.Login)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var identity = new CustomUserIdentity(user.Id, request.Login, user.Role);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );
            }

            user.Login = request.Login;
            user.Password = request.Password;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await _userBL.AddOrUpdateAsync(user);

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RecoveryPassword([FromBody] RecoveryPasswordRequest recoveryPasswordRequest)
        {
            var response = new BaseResponse();

            var userEmail = (await _userBL.GetAsync(new UsersSearchParams()
            {
                Email = recoveryPasswordRequest.Email,
                RegistrationStatus = UserRegistrationStatus.Confirmed
            })).Objects.FirstOrDefault();

            if (userEmail == null)
            {
                response.IsSuccess = false;
                response.TextError = "There is no user with such an email";
            }

            else
            {
                await _emailNotificationsBL.SendRecoveryPasswordEmailAsync(recoveryPasswordRequest.Email, userEmail);
                response.IsSuccess = true;
            }

            return Json(response);
        }

        public async Task<IActionResult> VerifyUser([FromQuery] string tokenKey)
        {
            var user = _encryption.Decrypt<User>(tokenKey);
            if (user != null)
            {
                var identity = new CustomUserIdentity(user.Id, user.Login, user.Role);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );

                return RedirectToAction("Index", "Account");
            }

            return RedirectToAction("Registration", "Account");
        }
    }
}

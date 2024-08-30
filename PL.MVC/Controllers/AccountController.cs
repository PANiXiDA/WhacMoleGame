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

namespace PL.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersBL _userBL;

        public AccountController(IUsersBL userBL)
        {
            _userBL = userBL;
        }

        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Public" });
            }

            return View();
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
    }
}

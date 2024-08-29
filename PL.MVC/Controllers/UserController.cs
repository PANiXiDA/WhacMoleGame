using BL.Interfaces;
using BL.Standard;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PL.MVC.Infrastructure.Claims;
using PL.MVC.Infrastructure.Models;
using PL.MVC.Infrastructure.Responses;
using PL.MVC.Infrastructure.ViewModel;
using System.Security.Claims;

namespace PL.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersBL _userBL;

        public UserController(IUsersBL userBL)
        {
            _userBL = userBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Authorization([FromBody] AuthorizationViewModel model)
        {
            var response = new ResponseAuthorizationModel();
            var user = await _userBL.VerifyPasswordAsync(model.Login, model.Password);

            if (user == null)
            {
                response.IsSuccess = false;
                response.TextError = "Указаны неверные данные для входа";
                return Json(response);
            }

            if (user.IsBlocked)
            {
                response.IsSuccess = false;
                response.TextError = "Пользователь заблокирован";
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


    }
}

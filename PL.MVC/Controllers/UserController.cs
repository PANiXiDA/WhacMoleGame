using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Common.SearchParams;
using Microsoft.AspNetCore.Authorization;
using PL.MVC.Infrastructure.Models;
using Common.Enums;

namespace PL.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersBL _userBL;

        public UserController(IUsersBL userBL)
        {
            _userBL = userBL;
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

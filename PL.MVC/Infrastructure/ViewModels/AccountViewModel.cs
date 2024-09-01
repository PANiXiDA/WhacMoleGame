using PL.MVC.Infrastructure.Models;

namespace PL.MVC.Infrastructure.ViewModels
{
    public class AccountViewModel
    {
        public UserModel User { get; set; } = new UserModel();
        public List<GameModel> Games { get; set; } = new List<GameModel>();
    }
}

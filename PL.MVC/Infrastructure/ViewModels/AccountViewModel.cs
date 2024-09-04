using PL.MVC.Infrastructure.Models;

namespace PL.MVC.Infrastructure.ViewModels
{
    public class AccountViewModel
    {
        public UserModel User { get; set; } = new UserModel();
        public int CountGames { get; set; }
        public int CountWins { get; set; }
        public int? MaxPointsCount { get; set; }
    }
}

using PL.MVC.Infrastructure.Models;

namespace PL.MVC.Infrastructure.ViewModels
{
    public class GameViewModel
    {
        public UserModel Player {  get; set; } = new UserModel();
        public int GameId {  get; set; }
    }
}

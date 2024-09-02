using PL.MVC.Infrastructure.Models;

namespace PL.MVC.Infrastructure.Requests
{
    public class GameOverRequest
    {
        public Dictionary<string, int> PlayerScores { get; set; } = new Dictionary<string, int>();
        public GameModel Game { get; set; } = new GameModel();
    }
}

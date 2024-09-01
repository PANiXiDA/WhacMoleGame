using InGame.IManagers;
using Entities;

namespace InGame.Managers
{
    public class GameManagerBL : IGameManagerBL
    {
        private static GameManagerBL _instance;
        public static GameManagerBL Instance => _instance ??= new GameManagerBL();

        public bool GameOver { get; private set; }
        public Game CurrentGame { get; private set; }
        public List<User> Players { get; private set; } = new List<User>();
        //public Mole CurrMole { get; private set; }
        //public Plant CurrPlant { get; private set; }

        private GameManagerBL() { }

        public void InitializeGame(Game game)
        {
            CurrentGame = game;
            Players = game.Sessions
            .Select(s => s.Player)
                .ToList();
        }

        public void UpdateGame()
        {
            if (GameOver) return;
        }
    }
}

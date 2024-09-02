using Entities;
using InGame.Models;
using System.Collections.Concurrent;

namespace InGame.IManagers
{
    public interface IGameManager
    {
        bool GameOver { get; }
        Game? CurrentGame { get; }
        List<User> Players { get; }
        Mole? CurrMole { get; }
        Plant? CurrPlant { get; }
        public ConcurrentDictionary<string, int> PlayerScores { get; set; }

        void InitializeGame(Game game);
        void UpdateGame();
        void PlayerMove(string playerLogin, int tileId);
        GameState GetGameState();
    }
}

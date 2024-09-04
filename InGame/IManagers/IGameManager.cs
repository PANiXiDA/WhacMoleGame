using Entities;
using InGame.Models;
using System.Collections.Concurrent;

namespace InGame.IManagers
{
    public interface IGameManager
    {
        bool GameOver { get; }
        Game? CurrentGame { get; set; }
        List<User> Players { get; set; }
        List<Mole> Moles { get; set; }
        List<Plant> Plants { get; set; }
        public ConcurrentDictionary<string, int> PlayerScores { get; set; }

        void InitializeGame(Game game);
        void UpdateGame();
        void PlayerMove(string playerLogin, int tileId);
        GameState GetGameState();
    }
}

using Entities;
using InGame.Models;

namespace InGame.IManagers
{
    public interface IGameManager
    {
        bool GameOver { get; }
        Game? CurrentGame { get; }
        List<User> Players { get; }
        Mole? CurrMole { get; }
        Plant? CurrPlant { get; }

        void InitializeGame(Game game);
        void UpdateGame();
        void PlayerMove(string playerLogin, int tileId);
        GameState GetGameState();
    }
}

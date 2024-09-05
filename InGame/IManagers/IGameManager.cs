using Entities;
using InGame.Models;

namespace InGame.IManagers
{
    public interface IGameManager
    {
        List<GameSession> GameSessions { get; }

        void InitializeGame(Game game);
        void UpdateGame();
        void PlayerMove(string playerLogin, int tileId, int gameId);
        GameState GetGameState(int gameId);
        void RemoveGameSession(int gameId);
    }
}

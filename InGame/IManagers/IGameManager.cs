using Entities;

namespace InGame.IManagers
{
    public interface IGameManagerBL
    {
        void InitializeGame(Game game);
        void UpdateGame();
        bool GameOver { get; }
        Game CurrentGame { get; }
    }
}

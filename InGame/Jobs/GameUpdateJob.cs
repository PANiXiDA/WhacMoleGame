using InGame.IManagers;
using Quartz;

namespace InGame.Jobs
{
    public class GameUpdateJob : IJob
    {
        private readonly IGameManager _gameManager;

        public GameUpdateJob(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public Task Execute(IJobExecutionContext context)
        {
            if (context != null && _gameManager.GameSessions != null && _gameManager.GameSessions.Any())
            {
                _gameManager.UpdateGame();
            }

            return Task.CompletedTask;
        }
    }
}

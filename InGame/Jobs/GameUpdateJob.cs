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
            if (context != null && _gameManager.CurrentGame != null && _gameManager.Moles != null
                && _gameManager.Moles.Any() &&_gameManager.Plants != null && _gameManager.Plants.Any())
            {
                _gameManager.UpdateGame();
            }

            return Task.CompletedTask;
        }
    }
}

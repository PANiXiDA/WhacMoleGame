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
            if (_gameManager.CurrentGame != null && _gameManager.CurrMole != null && _gameManager.CurrPlant != null)
            {
                _gameManager.UpdateGame();
            }

            return Task.CompletedTask;
        }
    }
}

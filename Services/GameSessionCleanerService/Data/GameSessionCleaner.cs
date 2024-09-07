using BL.Interfaces;
using Common.Configurations;
using Common.SearchParams;
using Entities;
using Microsoft.Extensions.Options;

namespace GameSessionCleanerService.Data
{
    public class GameSessionCleaner : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<GameSessionCleaner> _logger;
        private readonly GameSessionCleanerConfigurations _gameSessionCleanerConfigurations;

        public GameSessionCleaner(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<GameSessionCleaner> logger,
            IOptions<GameSessionCleanerConfigurations> gameSessionCleanerConfigurations)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _gameSessionCleanerConfigurations = gameSessionCleanerConfigurations.Value;           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game Session Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _gamesBL = scope.ServiceProvider.GetRequiredService<IGamesBL>();
                    var _sessionsBL = scope.ServiceProvider.GetRequiredService<ISessionsBL>();
                    try
                    {
                        var gamesForDeactive = (await _gamesBL.GetAsync(new GamesSearchParams
                        {
                            IsActive = true,
                            GameStartTime = DateTime.Now.AddDays(-1)
                        })).Objects.ToList();

                        List<Session> sesionsForDeactive = new List<Session>();
                        foreach (var game in gamesForDeactive)
                        {
                            game.GameEndTime = DateTime.Now;
                            game.IsActive = false;

                            var sessions = (await _sessionsBL.GetAsync(new SessionsSearchParams
                            {
                                GameId = game.Id
                            })).Objects.ToList();
                            sessions.ForEach(s => s.IsActive = false);
                            sesionsForDeactive.AddRange(sessions);
                        }

                        if (gamesForDeactive.Count > 0)
                        {
                            await _gamesBL.AddOrUpdateAsync(gamesForDeactive);
                            _logger.LogInformation($"Deactive {gamesForDeactive.Count} games.");
                        }

                        if (sesionsForDeactive.Count > 0)
                        {
                            await _sessionsBL.AddOrUpdateAsync(sesionsForDeactive);
                            _logger.LogInformation($"Deactive {sesionsForDeactive.Count} sessions.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while cleaning up games.");
                    }
                }

                await Task.Delay(TimeSpan.FromDays(_gameSessionCleanerConfigurations.DelayInDays), stoppingToken);
            }

            _logger.LogInformation("Game Session Service is stopping.");
        }
    }
}

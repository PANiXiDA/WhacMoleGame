using InGame.IManagers;
using InGame.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace InGame.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IGameManager, GameManager>();

            return services;
        }
    }
}

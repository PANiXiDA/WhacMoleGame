using BL.Interfaces;
using BL.Interfaces.InGame;
using BL.Standard.InGame;
using Microsoft.Extensions.DependencyInjection;


namespace BL.Standard
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IUsersBL, UsersBL>();
            services.AddScoped<IGamesBL, GamesBL>();
            services.AddScoped<ISessionsBL, SessionsBL>();
            services.AddScoped<IGameManagerBL, GameManagerBL>();

            return services;
        }
    }
}

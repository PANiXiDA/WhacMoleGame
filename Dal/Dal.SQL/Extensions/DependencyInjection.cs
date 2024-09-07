using Dal.DbModels;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.SQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DefaultDbContext>(config => config.UseNpgsql(configuration["ConnectionStrings:DefaultConnectionString"]));
            services.AddScoped<IUsersDal, UsersDal>();
            services.AddScoped<IGamesDal, GamesDal>();
            services.AddScoped<ISessionsDal, SessionsDal>();
            services.AddScoped<IFeedbacksDal, FeedbacksDal>();

            return services;
        }
    }
}

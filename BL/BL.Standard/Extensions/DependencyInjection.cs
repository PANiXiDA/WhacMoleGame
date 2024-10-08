﻿using BL.Interfaces;
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
            services.AddScoped<IFeedbacksBL, FeedbacksBL>();
            services.AddScoped<IEmailNotificationsBL, EmailNotificationsBL>();
            services.AddScoped<IConfirmationCodesBL, ConfirmationCodesBL>();

            return services;
        }
    }
}

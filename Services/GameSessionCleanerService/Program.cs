using BL.Standard;
using Dal.SQL;
using GameSessionCleanerService.Data;
using Common.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GameSessionCleanerConfigurations>(builder.Configuration.GetSection("GameSessionCleanerSettings"));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddHostedService<GameSessionCleaner>();

var app = builder.Build();

app.Run();

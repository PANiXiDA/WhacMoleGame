using BL.Standard;
using Dal.SQL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using InGame.Extensions;
using InGame.Jobs;
using Common.Configurations;
using Tools.SymmetricEncryption.AesEncryption;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AesEncryptionConfiguration>(builder.Configuration.GetSection("AesEncryptionSettings"));
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<AesEncryption>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("GameUpdateJob");

    q.AddJob<GameUpdateJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("GameUpdateJob-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever()));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllersWithViews();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddManagers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

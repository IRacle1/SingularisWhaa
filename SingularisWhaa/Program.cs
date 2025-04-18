using FluentValidation;

using SingularisWhaa.Models;
using SingularisWhaa.Models.User;
using SingularisWhaa.Services;
using SingularisWhaa.Services.Abstractions;
using SingularisWhaa.Services.Abstractions.Config;
using SingularisWhaa.Services.Config;

namespace SingularisWhaa;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IConfigManagerService, JsonConfigManagerService>();

        builder.Services.AddDbContext<ApplicationContext>();

        builder.Services.AddScoped<IValidator<UserDto>, UserValidator>();

        builder.Services.AddScoped<IUserCollectionService, EFUserCollectionService>();

        builder.Services.AddSingleton<ICronParserService, CronosCronParserService>();

        builder.Services.AddSingleton<IEmailService, FluentEmailService>();

        builder.Services.AddTransient<PeriodicEmailSender>();
        builder.Services.AddHostedService<CronPeriodicTaskService<PeriodicEmailSender>>();

        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.MapControllers();

        app.Run();
    }
}

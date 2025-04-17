using SingularisWhaa.Services.Abstractions;
using SingularisWhaa.Services.Abstractions.Config;

namespace SingularisWhaa.Services;

public class CronPeriodicTaskService<T> : BackgroundService, IDisposable where T : IPeriodicTask
{
    private readonly ILogger<CronPeriodicTaskService<T>> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly ICronParserService cronParserService;
    private readonly string cronExpression;

    private string ConfigName => $"CronTask_{typeof(T).Name}";

    public CronPeriodicTaskService(ILogger<CronPeriodicTaskService<T>> logger, IServiceProvider serviceProvider, IConfigManagerService configManager, ICronParserService cronParserService)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.cronParserService = cronParserService;

        ICategoryConfig config = configManager.GetConfig(ConfigName);

        if (!config.TryGet("CronExpression", out string? parsedExpression))
        {
            logger.LogWarning("Cron выражение (Поле 'CronExpression') в конфиге '{configname}' не задано, значение по умолчанию - раз в день.", ConfigName);
            parsedExpression = "0 0 * * *";
        }

        cronExpression = parsedExpression;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!cronParserService.TryNextOccurrenceDelta(cronExpression, out TimeSpan? deltaTime))
            {
                logger.LogWarning("Cron выражение '{expr}' инвалидно, или никогда не произойдет", cronExpression);
                return;
            }
            logger.LogInformation("Следующее исполнение Cron задачи, через {deltaTime}", deltaTime.Value);

            await Task.Delay(deltaTime.Value, stoppingToken);
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            try
            {
                T service = scope.ServiceProvider.GetRequiredService<T>();
                await service.DoTask(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка в дочернем сервисе Cron задачи");
            }
        }

        if (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Задача {nameof} получила запрос на отмену", nameof(CronPeriodicTaskService<T>));
        }
    }
}

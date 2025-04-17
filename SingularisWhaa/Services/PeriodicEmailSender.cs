
using SingularisWhaa.Models.User;
using SingularisWhaa.Services.Abstractions;
using SingularisWhaa.Services.Abstractions.Config;

namespace SingularisWhaa.Services;

/// <inheritdoc/>
public class PeriodicEmailSender : IPeriodicTask
{
    private readonly ILogger<PeriodicEmailSender> logger;
    private readonly IEmailService emailService;
    private readonly IUserCollectionService userCollection;

    private readonly string? templateName;

    public PeriodicEmailSender(ILogger<PeriodicEmailSender> logger, IConfigManagerService configManager, IEmailService emailService, IUserCollectionService userCollection)
    {
        this.logger = logger;
        this.emailService = emailService;
        this.userCollection = userCollection;
        ICategoryConfig config = configManager.GetConfig("EmailConfig");

        if (!config.TryGet("PeriodicTemplateName", out string? rawTemplateName))
        {
            logger.LogError("В конфиге 'EmailConfig' не указано поле 'PeriodicTemplateName', которое должно содержать название файла нужного шаблона!");
            logger.LogError("Сообщения просто не будут отправляться");
            return;
        }

        templateName = rawTemplateName;
    }

    /// <inheritdoc/>
    public async Task DoTask(CancellationToken cancellationToken)
    {
        if (templateName == null)
            return;

        foreach (UserDatabase user in await userCollection.GetAll())
        {
            await emailService.SendEmail(user, templateName);
        }
    }
}

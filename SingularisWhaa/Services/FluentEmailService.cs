using FluentEmail.Core;
using FluentEmail.Core.Defaults;
using FluentEmail.Liquid;

using Microsoft.Extensions.Options;

using SingularisWhaa.Models.User;
using SingularisWhaa.Services.Abstractions;
using SingularisWhaa.Services.Abstractions.Config;

namespace SingularisWhaa.Services;

public class FluentEmailService : IEmailService
{
    private readonly ILogger<FluentEmailService> logger;
    private readonly string rootEmailsPath;
    private readonly string senderEmail;
    private readonly string senderName;

    public FluentEmailService(ILogger<FluentEmailService> logger, IConfigManagerService configManager)
    {
        this.logger = logger;
        rootEmailsPath = Path.Combine(configManager.RootPath, "EmailTemplates");

        if (!Directory.Exists(rootEmailsPath))
        {
            logger.LogWarning("Папка с шаблонами для текста email'ов не создана, создание папки и стандартных шаблонов...");
            Directory.CreateDirectory(rootEmailsPath);

            SetupDefaultTemplates();
        }

        SetupEmailClient();

        ICategoryConfig config = configManager.GetConfig("EmailConfig");

        // Заглушка
        Email.DefaultSender = new SaveToDiskSender(configManager.RootPath);

        senderEmail = config.Get<string>("SenderEmail") ?? "from@from.from";
        senderName = config.Get<string>("SenderName") ?? "fromer";
    }

    private void SetupEmailClient()
    {
        Email.DefaultRenderer = new LiquidRenderer(Options.Create(new LiquidRendererOptions()));
    }

    private void SetupDefaultTemplates()
    {
        File.WriteAllText(Path.Combine(rootEmailsPath, "welcome.liquid"), """
            <h1>Привет '{{ Name }}' 🙏!</h1>
            <p>Твой email: '{{ Email }}'</p>
            <img src="https://i.pinimg.com/originals/82/1d/cb/821dcbe64cd2f1eba4413a4fb8d871d6.jpg"></img>
            """);

        File.WriteAllText(Path.Combine(rootEmailsPath, "info.liquid"), """
            <h1>Снова привет '{{ Name }}' 😊!</h1>
            <p>С момента регистрации прошло: '{{ DeltaTime }}' 🔥</p>
            <img src="https://static.wikia.nocookie.net/italianrot/images/1/10/Screenshot_2025-03-25_at_21.19.12.png/revision/latest?cb=20250325201958"></img>
            """);
    }

    /// <inheritdoc/>
    public async Task SendEmail(UserDatabase user, string template)
    {
        string path = Path.Combine(rootEmailsPath, template + ".liquid");

        if (!File.Exists(path))
        {
            logger.LogError("Шаблон '{templateName}' не найден по пути {rootPath}", template, rootEmailsPath);
            return;
        }

        string templateConent = await File.ReadAllTextAsync(path);

        FluentEmail.Core.Models.SendResponse response = await Email
            .From(senderEmail, senderName)
            .To(user.Email)
            .Subject("Singularis🐟")
            .UsingTemplate(templateConent, user)
            .SendAsync();

        if (response.Successful)
        {
            logger.LogInformation("Успешно отправлено сообщение '{template}' пользователю '{username}'({email})!", template, user.Name, user.Email);
            return;
        }

        logger.LogError("Cообщение '{template}' не отправлено пользователю '{username}'({email})!", template, user.Name, user.Email);

        logger.LogError("Ошибки:\n{errors}", string.Join(Environment.NewLine, response.ErrorMessages));
    }
}

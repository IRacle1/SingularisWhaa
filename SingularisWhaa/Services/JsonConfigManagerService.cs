using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;

using SingularisWhaa.Models.Abstractions;
using SingularisWhaa.Models.Config;
using SingularisWhaa.Services.Abstractions;

namespace SingularisWhaa.Services;

/// <inheritdoc/>
public class JsonConfigManagerService : IConfigManagerService
{
    private readonly ILogger<JsonConfigManagerService> logger;
    private readonly Dictionary<string, DocumentConfig> configCache = new();

    public JsonSerializerOptions Options { get; } = new()
    {
        AllowTrailingCommas = true,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    /// <inheritdoc/>
    public string RootPath { get; }

    public const string EnvName = "App_ConfigPath";

    public JsonConfigManagerService(IConfiguration configuration, ILogger<JsonConfigManagerService> logger)
    {
        this.logger = logger;
        RootPath = configuration.GetValue<string>(EnvName)!;

        if (string.IsNullOrWhiteSpace(RootPath))
        {
            logger.LogError("Путь к корневой папке конфигов Env '{envName}' не задан!", EnvName);
            throw new DirectoryNotFoundException();
        }

        if (!Directory.Exists(RootPath))
        {
            logger.LogWarning("Путь к корневой папке конфигов Env '{envName}' не существует, создание папки...", EnvName);
            Directory.CreateDirectory(RootPath);
        }
    }

    /// <inheritdoc/>
    public ICategoryConfig GetConfig(string category)
    {
        if (configCache.TryGetValue(category, out DocumentConfig? ret))
            return ret;

        string path = Path.Combine(RootPath, category + ".json");
        if (!File.Exists(path))
        {
            logger.LogWarning("Путь к конфигу Env '{envName}' не существует, создание файла конфига...", category);
            File.WriteAllText(path, "{}");
        }

        ret = new DocumentConfig(this, JsonDocument.Parse(File.ReadAllText(path)));
        configCache.Add(category, ret);
        return ret;
    }
}

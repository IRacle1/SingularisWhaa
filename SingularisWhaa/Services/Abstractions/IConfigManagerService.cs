using SingularisWhaa.Models.Abstractions;

namespace SingularisWhaa.Services.Abstractions;

public interface IConfigManagerService
{
    string RootPath { get; }

    ICategoryConfig GetConfig(string category);
}

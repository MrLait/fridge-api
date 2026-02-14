namespace Fridge.Infrastructure.Options;

public sealed class StorageOptions
{
    public const string SectionName = "Storage";
    public string RootPath { get; set; } = "App_Data/storage";
}
namespace Fridge.Application.Common.Interfaces;

public interface IFileStorage
{
    Task SaveAsync(string path, Stream content, CancellationToken ct);
    Task<Stream> OpenReadAsync(string path, CancellationToken ct);
    Task DeleteAsync(string path, CancellationToken ct);
    Task<bool> ExistsAsync(string path, CancellationToken ct);
}
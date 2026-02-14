using Fridge.Application.Common.Interfaces;

namespace Fridge.Infrastructure.Services;

public sealed class LocalFileStorage(string rootPath) : IFileStorage
{
    public Task DeleteAsync(string path, CancellationToken ct)
    {
        var fullPath = FullPath(path);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken ct)
        => Task.FromResult(File.Exists(FullPath(path)));

    public Task<Stream> OpenReadAsync(string path, CancellationToken ct)
        => Task.FromResult<Stream>(File.OpenRead(FullPath(path)));

    public async Task SaveAsync(string path, Stream content, CancellationToken ct)
    {
        var fullPath = FullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        await using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs, ct);
    }

    private string FullPath(string path)
        => Path.Combine(rootPath, path.Replace('/', Path.DirectorySeparatorChar));
}
using Fridge.Application.Common.Interfaces;

namespace Fridge.Application.Tests.Helpers;

public sealed class FakeFileStorage : IFileStorage
{
    public sealed record SaveCall(string Path, long BytesCopied);

    public List<SaveCall> SaveCalls { get; } = [];
    public List<string> DeleteCalls { get; } = [];
    public List<string> OpenReadCalls { get; } = [];

    public async Task SaveAsync(string path, Stream content, CancellationToken ct)
    {
        long copied = 0;
        var buffer = new byte[8192];
        int read;
        while ((read = await content.ReadAsync(buffer, ct)) > 0)
            copied += read;

        SaveCalls.Add(new SaveCall(path, copied));
    }

    public Task<Stream> OpenReadAsync(string path, CancellationToken ct)
    {
        OpenReadCalls.Add(path);
        return Task.FromResult<Stream>(new MemoryStream());
    }

    public Task DeleteAsync(string path, CancellationToken ct)
    {
        DeleteCalls.Add(path);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken ct)
        => Task.FromResult(true);
}
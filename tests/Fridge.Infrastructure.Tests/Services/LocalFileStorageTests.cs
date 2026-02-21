using FluentAssertions;
using Fridge.Infrastructure.Services;

namespace Fridge.Infrastructure.Tests.Services;

public class LocalFileStorageTests
{
    [Fact]
    public async Task SaveAsync_Should_Create_File_And_ExistsAsync_Should_Return_True()
    {
        var root = CreateTempDir();
        try
        {
            var storage = new LocalFileStorage(root);

            var path = "products/1/file.txt";
            var bytes = new byte[] { 1, 2, 3, 4, 5 };

            await using var content = new MemoryStream(bytes);

            await storage.SaveAsync(path, content, CancellationToken.None);

            var exists = await storage.ExistsAsync(path, CancellationToken.None);
            exists.Should().BeTrue();
        }
        finally
        {
            TryDeleteDir(root);
        }
    }

    [Fact]
    public async Task OpenReadAsync_Should_Return_Same_Content_As_Saved()
    {
        var root = CreateTempDir();

        try
        {
            var storage = new LocalFileStorage(root);
            var path = "products/1/file.bin";
            var bytes = new byte[] { 10, 20, 30 };
            await using var content = new MemoryStream(bytes);
            await storage.SaveAsync(path, content, CancellationToken.None);

            await using var readStream = await storage.OpenReadAsync(path, CancellationToken.None);
            await using var readContent = new MemoryStream();
            await readStream.CopyToAsync(readContent);

            readContent.ToArray().Should().Equal(bytes);
        }
        finally
        {
            TryDeleteDir(root);
        }
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_File_And_ExistsAsync_Should_Return_False()
    {
        var root = CreateTempDir();
        try
        {
            var storage = new LocalFileStorage(root);

            var path = "products/1/file.txt";
            await using (var content = new MemoryStream([1]))
                await storage.SaveAsync(path, content, CancellationToken.None);

            (await storage.ExistsAsync(path, CancellationToken.None)).Should().BeTrue();

            await storage.DeleteAsync(path, CancellationToken.None);

            (await storage.ExistsAsync(path, CancellationToken.None)).Should().BeFalse();
        }
        finally
        {
            TryDeleteDir(root);
        }
    }



    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "FridgeTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static void TryDeleteDir(string dir)
    {
        try
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive: true);
        }
        catch
        {
        }
    }
}

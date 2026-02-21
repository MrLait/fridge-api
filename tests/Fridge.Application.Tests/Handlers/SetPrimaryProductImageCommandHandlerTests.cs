using FluentAssertions;
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.ProductImages.Commands.SetPrimaryProductImage;
using Moq;

namespace Fridge.Application.Tests.Handlers;
public class SetPrimaryProductImageCommandHandlerTests
{
    [Fact]
    public async Task Should_Call_Service_SetPrimaryAsync()
    {
        // Given
        var service = new Mock<IProductImageService>(MockBehavior.Strict);

        var productId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        service.Setup(s => s.SetPrimaryAsync(productId, imageId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

        var handler = new SetPrimaryProductImageCommandHandler(service.Object);
        var command = new SetPrimaryProductImageCommand(productId, imageId);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then
        service.Verify(s => s.SetPrimaryAsync(productId, imageId, It.IsAny<CancellationToken>()), Times.Once);
        service.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Should_Propagate_Exception_From_Service()
    {
        // Given
        var service = new Mock<IProductImageService>();

        var ex = new KeyNotFoundException("boom");
        service.Setup(s => s.SetPrimaryAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(ex);

        var handler = new SetPrimaryProductImageCommandHandler(service.Object);
        var command = new SetPrimaryProductImageCommand(Guid.NewGuid(), Guid.NewGuid());

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("boom");
    }
}
using FluentAssertions;
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Maintenance.Commands.RestockZeroQuantity;
using Moq;

namespace Fridge.Application.Tests.Handlers;

public class RestockZeroQuantityCommandHandlerTests
{
    [Fact]
    public async Task Should_Call_Service_And_Return_Updated_Count()
    {
        // Given
        var service = new Mock<IRestockService>(MockBehavior.Strict);

        service
            .Setup(s => s.RestockZeroQuantityAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        var handler = new RestockZeroQuantityCommandHandler(service.Object);
        var command = new RestockZeroQuantityCommand();

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().Be(5);

        service.Verify(s => s.RestockZeroQuantityAsync(It.IsAny<CancellationToken>()), Times.Once);
        service.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Should_Propagate_Exception_From_Service()
    {
        // Given
        var service = new Mock<IRestockService>(MockBehavior.Strict);

        service
            .Setup(s => s.RestockZeroQuantityAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new RestockZeroQuantityCommandHandler(service.Object);
        var command = new RestockZeroQuantityCommand();

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("boom");

        service.Verify(s => s.RestockZeroQuantityAsync(It.IsAny<CancellationToken>()), Times.Once);
        service.VerifyNoOtherCalls();
    }
}
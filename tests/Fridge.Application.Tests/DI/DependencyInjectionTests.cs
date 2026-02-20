using FluentAssertions;
using FluentValidation;
using Fridge.Application;
using Fridge.Application.Common.Behaviors;
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.Fridges.Commands.CreateFridge;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fridge.Application.Tests.DI;

public class DependencyInjectionTests
{
    [Fact]
    public async Task ValidationBehavior_Should_Throw_ValidationException()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        using var sp = services.BuildServiceProvider();

        var validators = sp.GetServices<IValidator<CreateFridgeCommand>>();
        var behavior = new ValidationBehavior<CreateFridgeCommand, Guid>(validators);

        var cmd = new CreateFridgeCommand("", "Owner", Guid.Empty, null);

        Func<Task> act = () => behavior.Handle(
            cmd,
            next: _ => Task.FromResult(Guid.NewGuid()),
            cancellationToken: CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public void AddApplication_Should_Register_ValidationBehavior()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        using var sp = services.BuildServiceProvider();

        var behavior = sp.GetService<IPipelineBehavior<CreateFridgeCommand, Guid>>();

        behavior.Should().NotBeNull();
    }
}
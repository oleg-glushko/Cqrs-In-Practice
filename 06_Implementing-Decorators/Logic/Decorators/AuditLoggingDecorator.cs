using CSharpFunctionalExtensions;
using Logic.AppServices;
using System.Text.Json;

namespace Logic.Decorators;

public class AuditLoggingDecorator<TCommand>(ICommandHandler<TCommand> handler)
    : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> handler = handler;

    public Result Handle(TCommand command)
    {
        string commandJson = JsonSerializer.Serialize(command);

        // Use proper logging here
        Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

        return handler.Handle(command);
    }
}

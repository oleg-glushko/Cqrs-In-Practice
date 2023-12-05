using Api.Utils;
using Logic.AppServices;

namespace Api.Endpoints;

public static class UnregisterStudentEndpoint
{
    public static IResult Handler(ICommandHandler<UnregisterCommand> handler, long id)
    {
        var command = new UnregisterCommand { Id = id };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

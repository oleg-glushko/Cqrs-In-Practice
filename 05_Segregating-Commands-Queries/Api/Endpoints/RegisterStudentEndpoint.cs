using Api.Utils;
using Logic.Dtos;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class RegisterStudentEndpoint
{
    public static IResult Handler(ICommandHandler<RegisterCommand> handler, [FromBody] NewStudentDto dto)
    {
        var command = new RegisterCommand
        {
            Name = dto.Name,
            Email = dto.Email,
            Course1 = dto.Course1,
            Course1Grade = dto.Course1Grade,
            Course2 = dto.Course2,
            Course2Grade = dto.Course2Grade
        };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

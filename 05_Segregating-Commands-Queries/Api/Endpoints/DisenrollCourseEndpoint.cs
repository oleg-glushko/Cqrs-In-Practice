using Api.Utils;
using Logic.Dtos;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class DisenrollCourseEndpoint
{
    public static IResult Handler(ICommandHandler<DisenrollCommand> handler,
           long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
    {
        var command = new DisenrollCommand
        {
            Id = id,
            EnrollmentNumber = enrollmentNumber,
            Comment = dto.Comment
        };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

using Api.Utils;
using Logic.Dtos;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class EnrollCourseEndpoint
{
    public static IResult Handler(ICommandHandler<EnrollCommand> handler,
           long id, [FromBody] StudentEnrollmentDto dto)
    {
        var command = new EnrollCommand
        {
            Id = id,
            Course = dto.Course,
            Grade = dto.Grade
        };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

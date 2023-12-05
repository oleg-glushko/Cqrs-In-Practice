using Api.Utils;
using Logic.Dtos;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class TransferCourseEndpoint
{
    public static IResult Handler(ICommandHandler<TransferCommand> handler,
           long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
    {
        var command = new TransferCommand
        {
            Id = id,
            EnrollmentNumber = enrollmentNumber,
            Course = dto.Course,
            Grade = dto.Grade
        };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

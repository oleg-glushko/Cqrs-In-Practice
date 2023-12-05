using Api.Utils;
using Logic.Dtos;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class EditStudentPersonalInfoEndpoint
{
    public static IResult Handler(ICommandHandler<EditPersonalInfoCommand> handler,
           long id, [FromBody] StudentPersonalInfoDto dto)
    {
        var command = new EditPersonalInfoCommand
        {
            Email = dto.Email,
            Name = dto.Name,
            Id = id
        };
        CSharpFunctionalExtensions.Result result = handler.Handle(command);

        return EnvelopedResults.FromResult(result);
    }
}

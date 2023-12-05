using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class EditStudentPersonalInfoEndpoint
{
    public static IResult Handler(StudentRepository studentRepository,
           long id, [FromBody] StudentPersonalInfoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return EnvelopedResults.Error("Name shouldn't be empty");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return EnvelopedResults.Error("Email shouldn't be empty");

        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        student.Name = dto.Name;
        student.Email = dto.Email;

        studentRepository.Commit();

        return EnvelopedResults.Ok();
    }
}

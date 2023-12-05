using Api.Dtos;
using Api.Utils;
using Logic.Students;

namespace Api.Endpoints;

public static class GetStudentByIdEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, int id)
    {
        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");
        return EnvelopedResults.Ok(student.ConvertToStudentDto());
    }
}

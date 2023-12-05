using Api.Utils;
using Logic.Students;

namespace Api.Endpoints;

public static class UnregisterStudentEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, long id)
    {
        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        studentRepository.Delete(student);

        return EnvelopedResults.Ok();
    }
}

using Api.Dtos;
using Api.Utils;
using Logic.Students;

namespace Api.Endpoints;

public static class GetStudentsListEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, string enrolled, int? number)
    {
        IReadOnlyList<Student> students = studentRepository.GetList(enrolled, number);
        List<StudentDto> dtos = students.Select(x => x.ConvertToStudentDto()).ToList();
        return EnvelopedResults.Ok(dtos);
    }

    
}

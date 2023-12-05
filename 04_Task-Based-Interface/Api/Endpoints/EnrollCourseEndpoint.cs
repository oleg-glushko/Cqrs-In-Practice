using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class EnrollCourseEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, CourseRepository courseRepository,
           long id, [FromBody] StudentEnrollmentDto dto)
    {
        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        Course? course = courseRepository.GetByName(dto.Course);
        if (course == null)
            return EnvelopedResults.Error($"Course is incorrect: '{dto.Course}'");

        bool success = Enum.TryParse(dto.Grade, out Grade grade);
        if (!success)
            return EnvelopedResults.Error($"Grade is incorrect: '{dto.Grade}'");

        student.Enroll(course, grade);

        studentRepository.Commit();

        return EnvelopedResults.Ok();
    }
}

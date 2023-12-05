using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class RegisterStudentEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, CourseRepository courseRepository, [FromBody] NewStudentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return EnvelopedResults.Error("Name shouldn't be empty");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return EnvelopedResults.Error("Email shouldn't be empty");

        var student = new Student(dto.Name, dto.Email);

        if (dto.Course1 != null && dto.Course1Grade != null)
        {
            Course? course = courseRepository.GetByName(dto.Course1);
            if (course  != null)
                student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
        }

        if (dto.Course2 != null && dto.Course2Grade != null)
        {
            Course? course = courseRepository.GetByName(dto.Course2);
            if (course != null)
                student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
        }

        studentRepository.Save(student);

        return EnvelopedResults.Ok();
    }
}

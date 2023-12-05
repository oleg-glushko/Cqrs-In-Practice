using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class TransferCourseEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, CourseRepository courseRepository,
           long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Course))
            return EnvelopedResults.Error("Course is required");

        if (string.IsNullOrWhiteSpace(dto.Grade))
            return EnvelopedResults.Error("Grade is required");

        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        Course? course = courseRepository.GetByName(dto.Course);
        if (course == null)
            return EnvelopedResults.Error($"Course is incorrect: '{dto.Course}'");

        bool success = Enum.TryParse(dto.Grade, out Grade grade);
        if (!success)
            return EnvelopedResults.Error($"Grade is incorrect: '{dto.Grade}'");

        Enrollment? enrollment = student.GetEnrollment(enrollmentNumber);
        if (enrollment == null)
            return EnvelopedResults.Error($"No enrollment found with number: '{enrollmentNumber}'");

        enrollment.Update(course, grade);

        studentRepository.Commit();

        return EnvelopedResults.Ok();
    }
}

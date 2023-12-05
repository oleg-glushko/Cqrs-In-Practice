using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class DisenrollCourseEndpoint
{
    public static IResult Handler(StudentRepository studentRepository,
           long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Comment))
            return EnvelopedResults.Error("Disenrollment comment is required");

        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        Enrollment? enrollment = student.GetEnrollment(enrollmentNumber);
        if (enrollment == null)
            return EnvelopedResults.Error($"No enrollment found with number: '{enrollmentNumber}'");

        student.RemoveEnrollment(enrollment, dto.Comment);

        studentRepository.Commit();

        return EnvelopedResults.Ok();
    }
}

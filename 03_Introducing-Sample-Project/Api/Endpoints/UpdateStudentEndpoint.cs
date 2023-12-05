using Api.Dtos;
using Api.Utils;
using Logic.Students;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UpdateStudentEndpoint
{
    public static IResult Handler(StudentRepository studentRepository, CourseRepository courseRepository,
           long id, [FromBody] StudentDto dto)
    {
        Student? student = studentRepository.GetById(id);
        if (student == null)
            return EnvelopedResults.Error($"No student found for Id {id}");

        student.Name = dto.Name;
        student.Email = dto.Email;

        Enrollment? firstEnrollment = student.FirstEnrollment;
        Enrollment? secondEnrollment = student.SecondEnrollment;

        if (HasEnrollmentChanged(dto.Course1, dto.Course1Grade, firstEnrollment))
        {
            if (string.IsNullOrWhiteSpace(dto.Course1) && firstEnrollment != null) // Student disenrolls
            {
                if (string.IsNullOrWhiteSpace(dto.Course1DisenrollmentComment))
                    return EnvelopedResults.Error("Disenrollment comment is required");

                Enrollment? enrollment = firstEnrollment;
                student.RemoveEnrollment(enrollment);
                student.AddDisenrollmentComment(enrollment, dto.Course1DisenrollmentComment);
                
            }

            if (!string.IsNullOrWhiteSpace(dto.Course1))
            {
                if (string.IsNullOrWhiteSpace(dto.Course1Grade))
                    return EnvelopedResults.Error("Grade is required");

                Course? course = courseRepository.GetByName(dto.Course1);
                if (course == null)
                    return EnvelopedResults.Error("Course not found");

                if (firstEnrollment == null)
                {
                    // Student enrolls
                    student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
                }
                else
                {
                    // Student transfers
                    firstEnrollment.Update(course, Enum.Parse<Grade>(dto.Course1Grade));
                }
            }
        }

        if (HasEnrollmentChanged(dto.Course2, dto.Course2Grade, secondEnrollment))
        {
            if (string.IsNullOrWhiteSpace(dto.Course2) && secondEnrollment != null) // Student disenrolls
            {
                if (string.IsNullOrWhiteSpace(dto.Course2DisenrollmentComment))
                    return EnvelopedResults.Error("Disenrollment comment is required");

                Enrollment? enrollment = secondEnrollment;
                student.RemoveEnrollment(enrollment);
                student.AddDisenrollmentComment(enrollment, dto.Course2DisenrollmentComment);
            }

            if (!string.IsNullOrWhiteSpace(dto.Course2))
            {
                if (string.IsNullOrWhiteSpace(dto.Course2Grade))
                    return EnvelopedResults.Error("Grade is required");

                Course? course = courseRepository.GetByName(dto.Course2);
                if (course == null)
                    return EnvelopedResults.Error("Course not found");

                if (secondEnrollment == null)
                {
                    // Student enrolls
                    student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
                }
                else
                {
                    // Student transfers
                    secondEnrollment.Update(course, Enum.Parse<Grade>(dto.Course2Grade));
                }
            }
        }

        studentRepository.Save(student);

        return EnvelopedResults.Ok();
    }

    private static bool HasEnrollmentChanged(string? newCourseName, string? newGrade, Enrollment? enrollment)
    {
        if (string.IsNullOrWhiteSpace(newCourseName) && enrollment == null)
            return false;

        if (string.IsNullOrWhiteSpace(newCourseName) || enrollment == null)
            return true;

        return newCourseName != enrollment.Course.Name || newGrade != enrollment.Grade.ToString();
    }
}

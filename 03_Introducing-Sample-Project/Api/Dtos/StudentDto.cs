using Logic.Students;

namespace Api.Dtos;

public sealed class StudentDto
{
    public long Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;

    public string? Course1 { get; set; }
    public string? Course1Grade { get; set; }
    public string? Course1DisenrollmentComment { get; set; }
    public int? Course1Credits { get; set; }

    public string? Course2 { get; set; }
    public string? Course2Grade { get; set; }
    public string? Course2DisenrollmentComment { get; set; }
    public int? Course2Credits { get; set; }
}

public static class StudentDtoConverters {
    public static StudentDto ConvertToStudentDto(this Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Course1 = student.FirstEnrollment?.Course?.Name,
            Course1Grade = student.FirstEnrollment?.Grade.ToString(),
            Course1Credits = student.FirstEnrollment?.Course?.Credits,
            Course2 = student.SecondEnrollment?.Course?.Name,
            Course2Grade = student.SecondEnrollment?.Grade.ToString(),
            Course2Credits = student.SecondEnrollment?.Course?.Credits,
        };
    }
}
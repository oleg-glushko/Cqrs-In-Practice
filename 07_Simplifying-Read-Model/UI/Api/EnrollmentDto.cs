namespace UI.Api;

public sealed class EnrollmentDto
{
    public long Id { get; set; }
    public required string Course { get; set; }
    public required string Grade { get; set; }
}

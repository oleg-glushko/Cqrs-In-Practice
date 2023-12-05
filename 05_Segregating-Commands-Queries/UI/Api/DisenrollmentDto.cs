namespace UI.Api;

public sealed class DisenrollmentDto
{
    public long Id { get; set; }
    public int EnrollmentNumber { get; set; }
    public required string Comment { get; set; }
}
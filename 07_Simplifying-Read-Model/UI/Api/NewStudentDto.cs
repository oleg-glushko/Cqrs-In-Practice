namespace UI.Api;

public sealed class NewStudentDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Course1 { get; set; }
    public string? Course1Grade { get; set; }
    public string? Course2 { get; set; }
    public string? Course2Grade { get; set; }
}

namespace Logic.Dtos;

public sealed class StudentDto
{
    public long Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;

    public string? Course1 { get; set; }
    public string? Course1Grade { get; set; }
    public int? Course1Credits { get; set; }

    public string? Course2 { get; set; }
    public string? Course2Grade { get; set; }
    public int? Course2Credits { get; set; }
}

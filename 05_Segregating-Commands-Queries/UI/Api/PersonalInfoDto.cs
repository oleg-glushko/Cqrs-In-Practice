namespace UI.Api;

public sealed class PersonalInfoDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

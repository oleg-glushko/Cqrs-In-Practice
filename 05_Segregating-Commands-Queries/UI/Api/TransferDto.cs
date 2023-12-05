namespace UI.Api;

public sealed class TransferDto
{
    public long Id { get; set; }
    public int EnrollmentNumber { get; set; }
    public required string Course { get; set; }
    public required string Grade { get; set; }
}

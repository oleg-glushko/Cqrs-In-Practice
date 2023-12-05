namespace UI.Api;

public class Envelope<T>
{
    public T? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime TimeGenerated { get; set; }
}

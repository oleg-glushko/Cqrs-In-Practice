using Logic.Data;

namespace Logic.Students;

public class Course : Entity
{
    public string Name { get; protected set; } = string.Empty;
    public int Credits { get; protected set; }
}

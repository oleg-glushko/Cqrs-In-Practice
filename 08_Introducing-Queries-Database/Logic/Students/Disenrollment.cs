using Logic.Data;

namespace Logic.Students;

public class Disenrollment : Entity
{
    public virtual Student Student { get; protected set; } = default!;
    public virtual Course Course { get; protected set; } = default!;
    public DateTime DateTime { get; protected set; }
    public string Comment { get; protected set; } = string.Empty;

    protected Disenrollment()
    {
    }

    public Disenrollment(Student student, Course course, string comment)
    {
        Student = student;
        Course = course;
        Comment = comment;
        DateTime = DateTime.Now;
    }
}

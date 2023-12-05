using Logic.Data;

namespace Logic.Students;

public class Enrollment : Entity
{
    public virtual Student Student { get; protected set; } = default!;
    public virtual Course Course { get; protected set; } = default!;
    public Grade Grade { get; protected set; } = default!;

    protected Enrollment()
    {
    }

    public Enrollment(Student student, Course course, Grade grade)
    {
        Student = student;
        Course = course;
        Grade = grade;
    }

    public void Update(Course course, Grade grade)
    {
        Course = course;
        Grade = grade;
    }
}

public enum Grade
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    F = 5
}

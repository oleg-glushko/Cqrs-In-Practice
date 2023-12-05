using Logic.Data;

namespace Logic.Students;

public class Student : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    private readonly IList<Enrollment> _enrollments = [];
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
    public Enrollment? FirstEnrollment => GetEnrollment(0);
    public Enrollment? SecondEnrollment => GetEnrollment(1);

    private readonly IList<Disenrollment> _disenrollments = [];
    public virtual IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

    protected Student()
    {
    }

    public Student(string name, string email)
    {
        Name = name;
        Email = email;
    }

    private Enrollment? GetEnrollment(int index)
    {
        if (Enrollments.Count > index)
            return _enrollments[index];

        return null;
    }

    public void RemoveEnrollment(Enrollment enrollment)
    {
        _enrollments.Remove(enrollment);
    }

    public void AddDisenrollmentComment(Enrollment enrollment, string comment)
    {
        var disenrollment = new Disenrollment(this, enrollment.Course, comment);
        _disenrollments.Add(disenrollment);
    }

    public void Enroll(Course course, Grade grade)
    {
        if (_enrollments.Count >= 2)
            throw new Exception("Cannot have more than 2 enrollments");

        var enrollment = new Enrollment(this, course, grade);
        _enrollments.Add(enrollment);
    }
}

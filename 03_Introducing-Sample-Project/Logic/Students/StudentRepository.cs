using Logic.Data;
using Microsoft.EntityFrameworkCore;

namespace Logic.Students;

public sealed class StudentRepository(CqrsInPracticeContext context)
{
    private readonly CqrsInPracticeContext _context = context;

    public Student? GetById(long id)
    {
        return _context.Students.Find(id);
    }

    public IReadOnlyList<Student> GetList(string enrolledIn, int? numberOfCourses)
    {
        IQueryable<Student> query = _context.Students;

        if (!string.IsNullOrWhiteSpace(enrolledIn))
            query = query.Where(x => x.Enrollments.Any(e => e.Course.Name == enrolledIn));

        if (numberOfCourses != null)
            query = query.Where(x => x.Enrollments.Count == numberOfCourses);

        return [.. query];
    }

    public void Save(Student student)
    {
        _context.Attach(student);
        _context.SaveChanges();
    }

    public void Delete(Student student)
    {
        using var transaction = _context.Database.BeginTransaction();
        _context.Database.ExecuteSqlInterpolated($"DELETE FROM Disenrollment WHERE StudentId = {student.Id}");
        _context.Attach(student);
        _context.ChangeTracker.Entries<Student>().First().State = EntityState.Deleted;
        _context.SaveChanges();
        transaction.Commit();
    }
}

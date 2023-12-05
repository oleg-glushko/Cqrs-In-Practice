using Logic.Data;

namespace Logic.Students;

public class CourseRepository(CqrsInPracticeContext context)
{
    private readonly CqrsInPracticeContext _context = context;

    public Course? GetByName(string name)
    {
        return _context.Courses.SingleOrDefault(c => c.Name == name);
    }
}

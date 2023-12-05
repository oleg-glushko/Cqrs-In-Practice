using Logic.Students;
using Microsoft.EntityFrameworkCore;

namespace Logic.Data;

public class CqrsInPracticeContext(DbContextOptions<CqrsInPracticeContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().Navigation(p => p.Enrollments).AutoInclude();
        modelBuilder.Entity<Enrollment>().Navigation(p => p.Course).AutoInclude();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.ShortName());

            var keyName = entity.FindPrimaryKey()?.Properties.Select(x => x.Name).Single();
            if (keyName != null)
                entity.GetProperty(keyName).SetColumnName(entity.GetTableName() + keyName);
        }
    }
}

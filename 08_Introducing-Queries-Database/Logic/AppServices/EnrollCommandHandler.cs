using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class EnrollCommand : ICommand
{
    public long Id { get; init; }
    public required string Course { get; init; }
    public required string Grade { get; init; }

    [AuditLog]
    internal sealed class EnrollCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        : ICommandHandler<EnrollCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;
        private readonly CourseRepository courseRepository = courseRepository;

        public Result Handle(EnrollCommand command)
        {
            Student? student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            Course? course = courseRepository.GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            bool success = Enum.TryParse(command.Grade, out Grade grade);
            if (!success)
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            student.Enroll(course, grade);

            studentRepository.Commit();

            return Result.Success();
        }
    }
}

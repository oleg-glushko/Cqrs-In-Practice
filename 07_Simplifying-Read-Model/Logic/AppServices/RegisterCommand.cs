using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class RegisterCommand : ICommand
{
    public required string Name { get; init; }
    public required string Email { get; init; }

    public string? Course1 { get; init; }
    public string? Course1Grade { get; init; }
    public string? Course2 { get; init; }
    public string? Course2Grade { get; init; }

    [AuditLog]
    internal sealed class RegisterCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        : ICommandHandler<RegisterCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;
        private readonly CourseRepository courseRepository = courseRepository;

        public Result Handle(RegisterCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                return Result.Failure("Name shouldn't be empty");

            if (string.IsNullOrWhiteSpace(command.Email))
                return Result.Failure("Email shouldn't be empty");

            var student = new Student(command.Name, command.Email);

            if (command.Course1 != null && command.Course1Grade != null)
            {
                Course? course = courseRepository.GetByName(command.Course1);
                if (course != null)
                    student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
            }

            if (command.Course2 != null && command.Course2Grade != null)
            {
                Course? course = courseRepository.GetByName(command.Course2);
                if (course != null)
                    student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
            }

            studentRepository.Save(student);

            return Result.Success();
        }
    }
}

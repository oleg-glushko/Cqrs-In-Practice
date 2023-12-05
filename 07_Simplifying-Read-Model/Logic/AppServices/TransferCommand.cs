using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class TransferCommand : ICommand
{
    public long Id { get; init; }
    public int EnrollmentNumber { get; init; }
    public required string Course { get; init; }
    public required string Grade { get; init; }

    [AuditLog]
    internal sealed class TransferCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
        : ICommandHandler<TransferCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;
        private readonly CourseRepository courseRepository = courseRepository;

        public Result Handle(TransferCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Course))
                return Result.Failure("Course is required");

            if (string.IsNullOrWhiteSpace(command.Grade))
                return Result.Failure("Grade is required");

            Student? student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            Course? course = courseRepository.GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            bool success = Enum.TryParse(command.Grade, out Grade grade);
            if (!success)
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number: '{command.EnrollmentNumber}'");

            enrollment.Update(course, grade);

            studentRepository.Commit();

            return Result.Success();
        }
    }
}

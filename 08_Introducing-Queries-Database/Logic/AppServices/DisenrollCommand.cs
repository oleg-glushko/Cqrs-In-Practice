using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class DisenrollCommand : ICommand
{
    public long Id { get; init; }
    public int EnrollmentNumber { get; init; }
    public required string Comment { get; init; }

    [AuditLog]
    internal sealed class DisenrollCommandHandler(StudentRepository studentRepository)
        : ICommandHandler<DisenrollCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;

        public Result Handle(DisenrollCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Failure("Disenrollment comment is required");

            Student? student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            Enrollment? enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number: '{command.EnrollmentNumber}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            studentRepository.Commit();

            return Result.Success();
        }
    }
}



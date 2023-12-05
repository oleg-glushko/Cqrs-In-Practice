using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class EditPersonalInfoCommand : ICommand
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }

    [AuditLog]
    internal sealed class EditPersonalInfoCommandHandler(StudentRepository studentRepository)
    : ICommandHandler<EditPersonalInfoCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;

        public Result Handle(EditPersonalInfoCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                return Result.Failure("Name shouldn't be empty");

            if (string.IsNullOrWhiteSpace(command.Email))
                return Result.Failure("Email shouldn't be empty");

            Student? student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            student.Name = command.Name;
            student.Email = command.Email;

            studentRepository.Commit();

            return Result.Success();
        }
    }
}

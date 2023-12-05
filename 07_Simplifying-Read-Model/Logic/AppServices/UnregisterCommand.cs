using CSharpFunctionalExtensions;
using Logic.Decorators;
using Logic.Students;

namespace Logic.AppServices;

public sealed class UnregisterCommand : ICommand
{
    public long Id { get; init; }

    [AuditLog]
    internal sealed class UnregisterCommandHandler(StudentRepository studentRepository)
        : ICommandHandler<UnregisterCommand>
    {
        private readonly StudentRepository studentRepository = studentRepository;

        public Result Handle(UnregisterCommand command)
        {
            Student? student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            studentRepository.Delete(student);

            return Result.Success();
        }
    }
}

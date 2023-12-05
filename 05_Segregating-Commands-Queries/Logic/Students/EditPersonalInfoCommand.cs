using CSharpFunctionalExtensions;
using Logic.Dtos;

namespace Logic.Students;

public interface ICommand
{
}

public interface IQuery
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Result Handle(TCommand command);
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
{
    public TResult Handle(TQuery query);
}


public sealed class GetListQuery : IQuery
{
    public required string EnrolledIn { get; init; }
    public int? NumberOfCourses { get; init;  }
}

public sealed class GetListQueryHandler(StudentRepository studentRepository)
    : IQueryHandler<GetListQuery, List<StudentDto>>
{
    private readonly StudentRepository studentRepository = studentRepository;

    public List<StudentDto> Handle(GetListQuery query)
    {
        return studentRepository
            .GetList(query.EnrolledIn, query.NumberOfCourses)
            .Select(x => x.ConvertToStudentDto())
            .ToList();
    }
}

public sealed class EditPersonalInfoCommand : ICommand
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}

public sealed class EditPersonalInfoCommandHandler(StudentRepository studentRepository)
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

public sealed class RegisterCommand : ICommand {
    public required string Name { get; init; }
    public required string Email { get; init; }

    public string? Course1 { get; init; }
    public string? Course1Grade { get; init; }
    public string? Course2 { get; init; }
    public string? Course2Grade { get; init; }
}

public sealed class RegisterCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
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

public sealed class UnregisterCommand : ICommand
{
    public long Id { get; init; }
}

public sealed class UnregisterCommandHandler(StudentRepository studentRepository)
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

public sealed class EnrollCommand : ICommand
{
    public long Id { get; init; }
    public required string Course { get; init; }
    public required string Grade { get; init; }
}

public sealed class EnrollCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
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

public sealed class TransferCommand : ICommand
{
    public long Id { get; init; }
    public int EnrollmentNumber { get; init; }
    public required string Course { get; init; }
    public required string Grade { get; init; }
}

public sealed class TransferCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
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

public sealed class DisenrollCommand : ICommand
{
    public long Id { get; init; }
    public int EnrollmentNumber { get; init; }
    public required string Comment { get; init; }
}

public sealed class DisenrollCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository)
    : ICommandHandler<DisenrollCommand>
{
    private readonly StudentRepository studentRepository = studentRepository;
    private readonly CourseRepository courseRepository = courseRepository;

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

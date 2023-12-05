using CSharpFunctionalExtensions;
using Logic.Dtos;
using Logic.Students;

namespace Logic.AppServices;

public sealed class GetListQuery : IQuery
{
    public required string EnrolledIn { get; init; }
    public int? NumberOfCourses { get; init; }

    internal sealed class GetListQueryHandler(StudentRepository studentRepository)
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
}

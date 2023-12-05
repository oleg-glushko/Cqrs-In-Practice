using Dapper;
using Logic.Data;
using Logic.Dtos;
using Logic.Students;
using System.Data;

namespace Logic.AppServices;

public sealed class GetListQuery : IQuery
{
    public required string EnrolledIn { get; init; }
    public int? NumberOfCourses { get; init; }

    internal sealed class GetListQueryHandler(DapperProvider dapperProvider)
        : IQueryHandler<GetListQuery, List<StudentDto>>
    {
        private readonly DapperProvider dapperProvider = dapperProvider;

        public List<StudentDto> Handle(GetListQuery query)
        {
            string sql =
                """
                SELECT StudentID Id, Name, Email,
                    FirstCourseName Course1, FirstCourseCredits Course1Credits, FirstCourseGrade Course1Grade,
                    SecondCourseName Course2, SecondCourseCredits Course2Credits, SecondCourseGrade Course2Grade
                FROM dbo.Student
                WHERE (FirstCourseName = @Course
                        OR SecondCourseName = @Course
                        OR @Course IS NULL
                        OR @Course = '')
                    AND (NumberOfEnrollments = @Number
                        OR @Number IS NULL)
                ORDER BY StudentID ASC;
                """;

            using IDbConnection connection = dapperProvider.Connect();

            List<StudentDto> students = connection
                .Query<StudentDto>(sql, new
                {
                    Course = query.EnrolledIn,
                    Number = query.NumberOfCourses
                })
                .ToList();

            return students;
        }
    }
}

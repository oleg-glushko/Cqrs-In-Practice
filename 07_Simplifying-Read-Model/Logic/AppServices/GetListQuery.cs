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
                SELECT s.*, e.Grade, c.Name CourseName, c.Credits
                FROM dbo.Student s
                LEFT JOIN (
                    SELECT e.StudentID, COUNT(*) Number
                    FROM dbo.Enrollment e
                    GROUP BY e.StudentID
                ) t ON s.StudentID = t.StudentID
                LEFT JOIN dbo.Enrollment e ON e.StudentID = s.StudentID
                LEFT JOIN dbo.Course c ON e.CourseID = c.CourseID
                WHERE s.StudentID IN (
                	SELECT DISTINCT StudentID from Enrollment WHERE CourseID IN (
                		SELECT CourseID from Course 
                			WHERE (Name = @Course OR @Course IS NULL OR @Course = '')))
                    AND (ISNULL(t.Number, 0) = @Number OR @Number IS NULL)
                ORDER BY s.StudentID ASC
                """;

            using IDbConnection connection = dapperProvider.Connect();

            List<StudentInDB> students = connection
                .Query<StudentInDB>(sql, new
                {
                    Course = query.EnrolledIn,
                    Number = query.NumberOfCourses
                })
                .ToList();

            List<long> ids = students
                .GroupBy(x => x.StudentID)
                .Select(x => x.Key)
                .ToList();

            var result = new List<StudentDto>();

            foreach (long id in ids)
            {
                List<StudentInDB> data = students
                    .Where(x => x.StudentID == id)
                    .ToList();

                var dto = new StudentDto
                {
                    Id = data[0].StudentID,
                    Name = data[0].Name,
                    Email = data[0].Email,
                    Course1 = data[0].CourseName,
                    Course1Credits = data[0].Credits,
                    Course1Grade = data[0].Grade.ToString()
                };
                if (data.Count > 1)
                {
                    dto.Course2 = data[1].CourseName;
                    dto.Course1Credits = data[1].Credits;
                    dto.Course1Grade = data[1].Grade.ToString();
                }

                result.Add(dto);
            }

            return result;
        }
    }

    private record StudentInDB(long StudentID, string Name, string Email,
        Grade? Grade, string? CourseName, int? Credits);
}

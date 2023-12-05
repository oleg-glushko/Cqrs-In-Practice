using Api.Utils;
using Logic.AppServices;
using Logic.Dtos;

namespace Api.Endpoints;

public static class GetStudentsListEndpoint
{
    public static IResult Handler(IQueryHandler<GetListQuery, List<StudentDto>> handler,
        string enrolled, int? number)
    {
        var query = new GetListQuery
        {
            EnrolledIn = enrolled,
            NumberOfCourses = number
        };
        List<StudentDto> list = handler.Handle(query);
        return EnvelopedResults.Ok(list);
    }

    
}

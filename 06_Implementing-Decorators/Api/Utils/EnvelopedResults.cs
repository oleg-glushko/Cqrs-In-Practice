namespace Api.Utils;

public static class EnvelopedResults
{
    public static IResult Ok() => Results.Ok(Envelope.Ok());

    public static IResult Ok<T>(T result) => Results.Ok(Envelope.Ok(result));

    public static IResult Error(string errorMessage) => Results.BadRequest(Envelope.Error(errorMessage));

    public static IResult FromResult(CSharpFunctionalExtensions.Result result) =>
        result.IsSuccess ? Ok() : Error(result.Error);
}

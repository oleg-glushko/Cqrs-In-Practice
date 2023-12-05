using CSharpFunctionalExtensions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace UI.Api;

public static class StudentEnpointsHelper
{
    private const string apiPrefix = "api/students";

    public static async Task<IReadOnlyList<StudentDto>> GetAllStudents(this HttpClient client, string enrolledIn,
        string numberOfCourses)
    {
        var requestUrl = $"{apiPrefix}?enrolled={enrolledIn}";
        if (numberOfCourses != "")
            requestUrl += $"&number={numberOfCourses}";
        Result<List<StudentDto>?> response = await SendRequest<List<StudentDto>>(client,
            requestUrl, HttpMethod.Get);
        return response.Value ?? [];
    }

    public static async Task<Result<StudentDto?>> GetStudent(this HttpClient client, long id)
    {
        Result<StudentDto?> result = await SendRequest<StudentDto>(client, $"{apiPrefix}/{id}", HttpMethod.Get);
        return result;
    }

    public static async Task<Result> CreateStudent(this HttpClient client, StudentDto dto)
    {
        Result result = await SendRequest<string>(client, $"{apiPrefix}", HttpMethod.Post, dto);
        return result;
    }

    public static async Task<Result> UpdateStudent(this HttpClient client, StudentDto dto)
    {
        Result result = await SendRequest<string>(client, $"{apiPrefix}/{dto.Id}", HttpMethod.Put, dto);
        return result;
    }

    private static async Task<Result<T?>> SendRequest<T>(HttpClient client, string url, HttpMethod method,
        object? content = null) where T : class
    {
        var request = new HttpRequestMessage(method, url);
        if (content != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        }

        HttpResponseMessage message = await client.SendAsync(request);

        if (message.StatusCode == HttpStatusCode.InternalServerError)
            throw new Exception(await message.Content.ReadAsStringAsync());

        var envelope = await message.Content.ReadFromJsonAsync<Envelope<T>>();

        if (!message.IsSuccessStatusCode)
            return Result.Failure<T?>(envelope?.ErrorMessage ?? "Unable to deserialize a response");

        T? result = envelope?.Result;

        if (result == null && typeof(T) == typeof(string))
            result = string.Empty as T;
        return Result.Success(result);
    }

}

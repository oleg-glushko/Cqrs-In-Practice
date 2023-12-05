using Api.Components;
using Api.Endpoints;
using Api.Utils;
using Blazored.Toast;
using Logic.Data;
using Logic.Students;
using Microsoft.EntityFrameworkCore;
using UI.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("", options =>
{
    options.BaseAddress = new("https://localhost:7190");
});
builder.Services.AddDbContext<CqrsInPracticeContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CqrsInPracticeDb");
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddBlazoredToast();

builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<CourseRepository>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseMiddleware<ExceptionHandler>();

var students = app.MapGroup("api/students");
students.MapGet("", GetStudentsListEndpoint.Handler);
students.MapGet("{id}", GetStudentByIdEndpoint.Handler);
students.MapPost("", CreateStudentEndpoint.Handler);
students.MapDelete("{id}", DeleteStudentEndpoint.Handler);
students.MapPut("{id}", UpdateStudentEndpoint.Handler);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(StudentList).Assembly);

app.Run();

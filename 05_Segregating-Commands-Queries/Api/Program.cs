using Api.Components;
using Api.Endpoints;
using Api.Utils;
using Blazored.Toast;
using Logic.Data;
using Logic.Dtos;
using Logic.Students;
using Microsoft.EntityFrameworkCore;
using UI.Students;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("", options =>
{
    options.BaseAddress = new(builder.Configuration["APIBaseAddress"]
        ?? throw new Exception("An API's base address configuration is missing"));
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

builder.Services.AddScoped<ICommandHandler<EditPersonalInfoCommand>, EditPersonalInfoCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetListQuery, List<StudentDto>>, GetListQueryHandler>();
builder.Services.AddScoped<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UnregisterCommand>, UnregisterCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
builder.Services.AddScoped<ICommandHandler<TransferCommand>, TransferCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();

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
students.MapPost("", RegisterStudentEndpoint.Handler);
students.MapDelete("{id}", UnregisterStudentEndpoint.Handler);
students.MapPut("{id}", EditStudentPersonalInfoEndpoint.Handler);

var enrollments = students.MapGroup("{id}/enrollments");
enrollments.MapPost("", EnrollCourseEndpoint.Handler);
enrollments.MapPut("{enrollmentNumber}", TransferCourseEndpoint.Handler);
enrollments.MapPost("{enrollmentNumber}/deletion", DisenrollCourseEndpoint.Handler);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(StudentListPage).Assembly);

app.Run();

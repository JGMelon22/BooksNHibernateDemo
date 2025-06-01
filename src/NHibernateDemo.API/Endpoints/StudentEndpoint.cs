using MiniValidation;
using NetDevPack.SimpleMediator;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Shared;

namespace NHibernateDemo.API.Endpoints;

public static class StudentEndpoint
{
    public static void MapStudentsRoute(this IEndpointRouteBuilder app)
    {
        app.MapGet("/students", GetStudentsListAsync)
            .WithName("GetStudentsList")
            .WithOpenApi();

        app.MapGet("/student/{id}", GetStudentAsync)
            .WithName("GetStudent")
            .WithOpenApi();

        app.MapPost("/student", AddStudentAsync)
            .WithName("AddStudent")
            .WithOpenApi();

        app.MapPatch("/student/{id}", UpdateStudentAsync)
            .WithName("UpdateStudent")
            .WithOpenApi();

        app.MapDelete("/student/{id}", RemoveStudentAsync)
            .WithName("RemoveStudent")
            .WithOpenApi();
    }

    private static async Task<IResult> GetStudentsListAsync(IMediator mediator)
    {
        Result<IEnumerable<StudentResponse>> students = await mediator.Send(new GetStudentsQuery());

        return students.Data != null && students.Data!.Any()
            ? Results.Ok(students)
            : Results.NoContent();
    }

    private static async Task<IResult> GetStudentAsync(IMediator mediator, int id)
    {
        Result<StudentResponse> student = await mediator.Send(new GetStudentByIdQuery(id));

        return student.Data != null
            ? Results.Ok(student)
            : Results.NotFound(student);
    }

    private static async Task<IResult> AddStudentAsync(IMediator mediator, StudentRequest student)
    {
        if (!MiniValidator.TryValidate(student, out IDictionary<string, string[]> errors))
            return Results.ValidationProblem(errors);

        Result<bool> success = await mediator.Send(new CreateStudentCommand(student));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }

    private static async Task<IResult> UpdateStudentAsync(IMediator mediator, int id, StudentRequest student)
    {
        if (!MiniValidator.TryValidate(student, out IDictionary<string, string[]> errors))
            return Results.ValidationProblem(errors);

        Result<bool> success = await mediator.Send(new UpdateStudentCommand(id, student));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }

    private static async Task<IResult> RemoveStudentAsync(IMediator mediator, int id)
    {
        Result<bool> success = await mediator.Send(new RemoveStudentCommand(id));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }
}

using NetDevPack.SimpleMediator;
using NHibernateDemo.Application.Queries;
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
    }

    private static async Task<IResult> GetStudentsListAsync(IMediator mediator)
    {
        Result<IEnumerable<StudentResponse>> students = await mediator.Send(new GetStudentsQuery());

        return students.Data != null || students.Data!.Any()
            ? Results.Ok(students)
            : Results.BadRequest(students);
    }
}
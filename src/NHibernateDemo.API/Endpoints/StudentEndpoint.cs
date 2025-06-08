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
        RouteGroupBuilder group = app.MapGroup("/students");

        group.MapGet("/", GetStudentsListAsync)
            .WithName("GetStudentsList")
            .WithOpenApi();

        group.MapGet("/{id}", GetStudentAsync)
            .WithName("GetStudent")
            .WithOpenApi();

        group.MapPost("/", AddStudentAsync)
            .WithName("AddStudent")
            .WithOpenApi();

        group.MapPatch("/{id}", UpdateStudentAsync)
            .WithName("UpdateStudent")
            .WithOpenApi();

        group.MapDelete("/{id}", RemoveStudentAsync)
            .WithName("RemoveStudent")
            .WithOpenApi();
    }

    /// <summary>
    ///     Retrieves a list of all registered students.
    /// </summary>
    /// <param name="mediator">The mediator used to send the query.</param>
    /// <returns>A list of students or a no content response.</returns>
    /// <response code="200">Returns the list of students.</response>
    /// <response code="204">Returns if no students are found.</response>
    /// <remarks>
    /// ### Example Response (200 OK)
    /// ```json
    /// {
    ///   "data": [
    ///     {
    ///       "id": 1,
    ///       "name": "Jane Doe",
    ///       "email": "jane.doe@example.com",
    ///       "course": "Computer Science",
    ///       "gender": "Female"
    ///     },
    ///     {
    ///       "id": 2,
    ///       "name": "John Smith",
    ///       "email": "john.smith@example.com",
    ///       "course": "Engineering",
    ///       "gender": "Male"
    ///     }
    ///   ],
    ///   "isSuccess": true,
    ///   "message": ""
    /// }
    /// ```
    /// </remarks>
    internal static async Task<IResult> GetStudentsListAsync(IMediator mediator)
    {
        Result<IEnumerable<StudentResponse>> students = await mediator.Send(new GetStudentsQuery());

        return students.Data != null && students.Data!.Any()
            ? Results.Ok(students)
            : Results.NoContent();
    }

    /// <summary>
    ///     Retrieves a student's information by their unique identifier.
    /// </summary>
    /// <param name="mediator">The mediator used to send the query.</param>
    /// <param name="id">The ID of the student to retrieve.</param>
    /// <returns>The student information for the specified ID.</returns>
    /// <response code="200">Returns the student information if found.</response>
    /// <response code="404">Returns if the student is not found.</response>
    /// <remarks>
    /// ### Example Response (200 OK)
    /// ```json
    /// {
    ///   "data": {
    ///     "id": 1,
    ///     "name": "Jane Doe",
    ///     "email": "jane.doe@example.com",
    ///     "course": "Computer Science",
    ///     "gender": "Female"
    ///   },
    ///   "isSuccess": true,
    ///   "message": ""
    /// }
    /// ```
    ///
    /// ### Example Response (404 Not Found)
    /// ```json
    /// {
    ///   "data": null,
    ///   "isSuccess": false,
    ///   "message": "Student with Id 99 not found!"
    /// }
    /// ```
    /// </remarks>
    internal static async Task<IResult> GetStudentAsync(IMediator mediator, int id)
    {
        Result<StudentResponse> student = await mediator.Send(new GetStudentByIdQuery(id));

        return student.Data != null
            ? Results.Ok(student)
            : Results.NotFound(student);
    }

    /// <summary>
    ///     Adds a new student to the system.
    /// </summary>
    /// <param name="mediator">The mediator used to send the command.</param>
    /// <param name="student">The student data to create.</param>
    /// <returns>The result of the operation.</returns>
    /// <response code="200">Returns if the student was successfully created.</response>
    /// <response code="400">Returns if the request data is invalid or creation failed.</response>
    /// <remarks>
    /// ### Example Request
    /// ```json
    /// {
    ///   "name": "Emily Turner",
    ///   "email": "emily.turner@example.com",
    ///   "course": "Business Administration",
    ///   "gender": "Female"
    /// }
    /// ```
    ///
    /// ### Example Response (200 OK)
    /// ```json
    /// {
    ///   "data": true,
    ///   "isSuccess": true,
    ///   "message": ""
    /// }
    /// ```
    ///
    /// ### Example Response (400 Bad Request)
    /// ```json
    /// {
    ///   "data": false,
    ///   "isSuccess": false,
    ///   "message": "A student with the given email already exists."
    /// }
    /// ```
    /// </remarks>
    internal static async Task<IResult> AddStudentAsync(IMediator mediator, StudentRequest student)
    {
        if (!MiniValidator.TryValidate(student, out IDictionary<string, string[]> errors))
            return Results.ValidationProblem(errors);

        Result<bool> success = await mediator.Send(new CreateStudentCommand(student));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }

    /// <summary>
    ///     Updates an existing student's information.
    /// </summary>
    /// <param name="mediator">The mediator used to send the command.</param>
    /// <param name="id">The ID of the student to update.</param>
    /// <param name="student">The updated student data.</param>
    /// <returns>The result of the update operation.</returns>
    /// <response code="200">Returns if the update was successful.</response>
    /// <response code="400">Returns if validation fails or update fails.</response>
    /// <remarks>
    /// ### Example Request
    /// ```json
    /// {
    ///   "name": "Emily Turner",
    ///   "email": "emily.t@example.com",
    ///   "course": "Marketing",
    ///   "gender": "Female"
    /// }
    /// ```
    ///
    /// ### Example Response (200 OK)
    /// ```json
    /// {
    ///   "data": true,
    ///   "isSuccess": true,
    ///   "message": ""
    /// }
    /// ```
    /// </remarks>
    internal static async Task<IResult> UpdateStudentAsync(IMediator mediator, int id, StudentRequest student)
    {
        if (!MiniValidator.TryValidate(student, out IDictionary<string, string[]> errors))
            return Results.ValidationProblem(errors);

        Result<bool> success = await mediator.Send(new UpdateStudentCommand(id, student));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }

    /// <summary>
    ///     Removes a student from the system by their unique ID.
    /// </summary>
    /// <param name="mediator">The mediator used to send the command.</param>
    /// <param name="id">The ID of the student to remove.</param>
    /// <returns>A result indicating whether the deletion was successful.</returns>
    /// <response code="200">Returns if the student was successfully removed.</response>
    /// <response code="400">Returns if the student could not be removed.</response>
    /// <remarks>
    /// ### Example Response (200 OK)
    /// ```json
    /// {
    ///   "data": true,
    ///   "isSuccess": true,
    ///   "message": ""
    /// }
    /// ```
    ///
    /// ### Example Response (400 Bad Request)
    /// ```json
    /// {
    ///   "data": false,
    ///   "isSuccess": false,
    ///   "message": "Student with Id 99 not found!"
    /// }
    /// ```
    /// </remarks>
    internal static async Task<IResult> RemoveStudentAsync(IMediator mediator, int id)
    {
        Result<bool> success = await mediator.Send(new RemoveStudentCommand(id));

        return success.IsSuccess != false
            ? Results.Ok(success)
            : Results.BadRequest(success);
    }
}

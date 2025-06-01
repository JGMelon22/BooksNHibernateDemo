using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using NetDevPack.SimpleMediator;
using NHibernateDemo.API.Endpoints;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Shared;
using Shouldly;

namespace NHibernateDemo.API.UnitTests.Endpoints;

public class StudentEndpointTests
{
    [Fact]
    public async Task Should_Return200Ok_When_StudentsListIsNotEmpty()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        IEnumerable<StudentResponse> students =
        [
           new() { Id = 1, Name = "Alice Johnson", Email = "alice.johnson@example.com", Course = "Computer Science", Gender = "Female" },
           new() { Id = 2, Name = "Bob Smith", Email = "bob.smith@example.com", Course = "Mechanical Engineering", Gender = "Male" },
           new() { Id = 3, Name = "Clara Lee", Email = "clara.lee@example.com", Course = "Electrical Engineering", Gender = "Female" },
           new() { Id = 4, Name = "David Kim", Email = "david.kim@example.com", Course = "Business Administration", Gender = "Male" },
           new() { Id = 5, Name = "Ella Martinez", Email = "ella.martinez@example.com", Course = "Psychology", Gender = "Female" }
        ];

        Result<IEnumerable<StudentResponse>> result = Result<IEnumerable<StudentResponse>>.Success(students);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentsQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentsListAsync(mediator.Object);

        // Assert
        result.Data.ShouldNotBeNull();
        response.ShouldBeOfType<Ok<Result<IEnumerable<StudentResponse>>>>();

        mediator.Verify(x => x.Send(
            It.IsAny<GetStudentsQuery>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return204NoContent_When_StudentsListIsEmpty()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        IEnumerable<StudentResponse> students = [];

        Result<IEnumerable<StudentResponse>> result = Result<IEnumerable<StudentResponse>>.Success(students);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentsQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentsListAsync(mediator.Object);

        // Assert
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        response.ShouldBeOfType<NoContent>();

        mediator.Verify(x => x.Send(
            It.IsAny<GetStudentsQuery>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return200Ok_When_StudentIdIsFound()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentResponse student = new()
        {
            Id = 1,
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            Course = "Computer Science",
            Gender = "Female"
        };

        Result<StudentResponse> result = Result<StudentResponse>.Success(student);

        mediator
            .Setup(x => x.Send(It.Is<GetStudentByIdQuery>(q => q.Id == 1), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentAsync(mediator.Object, 1);

        // Assert
        result.Data.ShouldNotBeNull();
        response.ShouldBeOfType<Ok<Result<StudentResponse>>>();

        mediator.Verify(x => x.Send(
            It.Is<GetStudentByIdQuery>(q => q.Id == 1),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return404NotFound_When_StudentsIsNotFound()
    {
        // Arrange
        Mock<IMediator> mediator = new();

        Result<StudentResponse> result = Result<StudentResponse>.Failure("Student with Id 2 not found!");

        mediator
            .Setup(x => x.Send(It.Is<GetStudentByIdQuery>(q => q.Id == 2), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentAsync(mediator.Object, 2);

        // Assert
        result.Data.ShouldBeNull();
        response.ShouldBeOfType<NotFound<Result<StudentResponse>>>();

        mediator.Verify(x => x.Send(
            It.Is<GetStudentByIdQuery>(q => q.Id == 2),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return200Ok_When_PassedStudentToBeCreatedIsValid()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentRequest student = new("Alice Johnson", "alice.johnson@example.com", "Computer Science", "Female");

        Result<bool> result = Result<bool>.Success(true);

        mediator
            .Setup(x => x.Send(It.IsAny<CreateStudentCommand>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.AddStudentAsync(mediator.Object, student);

        // Assert
        result.Data.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
        response.ShouldBeOfType<Ok<Result<bool>>>();

        mediator.Verify(x => x.Send(
            It.IsAny<CreateStudentCommand>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return400BadRequest_When_PassedStudentToBeCreatedIsNotValid()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentRequest student = new("0", "invalid.email@example.com", "Computer Science", "Female");

        Result<bool> result = Result<bool>.Failure(string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<CreateStudentCommand>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.AddStudentAsync(mediator.Object, student);

        // Assert
        result.Data.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
        response.ShouldBeOfType<ProblemHttpResult>();

        mediator.Verify(x => x.Send(
            It.IsAny<CreateStudentCommand>(),
            default), Times.Never);
    }

    [Fact]
    public async Task Should_Return_200Ok_When_StudentIdIsFoundAndSuccessfullyUpdated()
    {
        // Arrange
        Mock<IMediator> mediator = new();

        StudentRequest studentRequest = new("Alice Johnson", "alice.johnson@example.com", "Computer Science", "Female");
        Result<bool> result = Result<bool>.Success(true);

        mediator
            .Setup(x => x.Send(It.Is<UpdateStudentCommand>(c => c.Id == 12), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.UpdateStudentAsync(mediator.Object, 12, studentRequest);

        // Assert
        result.Data.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
        response.ShouldBeOfType<Ok<Result<bool>>>();

        mediator.Verify(x => x.Send(
            It.Is<UpdateStudentCommand>(cmd => cmd.Id == 12),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return_404NotFound_When_StudentIdToBeUpdatedIsNotFound()
    {
        // Arrange
        Mock<IMediator> mediator = new();

        StudentRequest studentRequest = new("Alice Johnson", "alice.johnson@example.com", "Computer Science", "Female");
        Result<bool> result = Result<bool>.Failure("Student with Id 12 not found!");

        mediator
            .Setup(x => x.Send(It.Is<UpdateStudentCommand>(c => c.Id == 12), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.UpdateStudentAsync(mediator.Object, 12, studentRequest);

        // Assert
        result.Data.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
        response.ShouldBeOfType<BadRequest<Result<bool>>>();

        mediator.Verify(x => x.Send(
            It.Is<UpdateStudentCommand>(cmd => cmd.Id == 12),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return400BadRequest_When_PassedStudentToBeUpdatedIsNotValid()
    {
        // Arrange
        Mock<IMediator> mediator = new();

        StudentRequest studentRequest = new("0", "x", "y", "z");
        Result<bool> result = Result<bool>.Failure(string.Empty);

        mediator
            .Setup(x => x.Send(It.Is<UpdateStudentCommand>(c => c.Id == 12), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.UpdateStudentAsync(mediator.Object, 12, studentRequest);

        // Assert
        result.Data.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
        response.ShouldBeOfType<ProblemHttpResult>();

        mediator.Verify(x => x.Send(
            It.Is<UpdateStudentCommand>(cmd => cmd.Id == 12),
            default), Times.Never);
    }

    [Fact]
    public async Task Should_Return200Ok_When_StudentIsSuccessfullyDeleted()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        Result<bool> result = Result<bool>.Success(true);

        mediator
            .Setup(x => x.Send(It.Is<RemoveStudentCommand>(c => c.Id == 12), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.RemoveStudentAsync(mediator.Object, 12);

        // Assert
        result.Data.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
        response.ShouldBeOfType<Ok<Result<bool>>>();

        mediator.Verify(x => x.Send(
            It.Is<RemoveStudentCommand>(cmd => cmd.Id == 12),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Return400BadRequest_When_StudentToBeDeletedIsNotFound()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        Result<bool> result = Result<bool>.Failure(string.Empty);

        mediator
            .Setup(x => x.Send(It.Is<RemoveStudentCommand>(c => c.Id == 12), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.RemoveStudentAsync(mediator.Object, 12);

        // Assert
        result.Data.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
        response.ShouldBeOfType<BadRequest<Result<bool>>>();

        mediator.Verify(x => x.Send(
            It.Is<RemoveStudentCommand>(cmd => cmd.Id == 12),
            default), Times.Once);
    }
}

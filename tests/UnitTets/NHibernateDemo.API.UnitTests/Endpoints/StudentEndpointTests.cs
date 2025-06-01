using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetDevPack.SimpleMediator;
using NHibernateDemo.API.Endpoints;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
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

        Result<IEnumerable<StudentResponse>> result = new(students, true, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentsQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentsListAsync(mediator.Object);

        // Assert
        result.Data.ShouldNotBeNull();
        response.ShouldBeOfType<Ok<Result<IEnumerable<StudentResponse>>>>();
    }

    [Fact]
    public async Task Should_Return204NoContent_When_StudentsListIsEmpty()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        IEnumerable<StudentResponse> students = [];

        Result<IEnumerable<StudentResponse>> result = new(students, true, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentsQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentsListAsync(mediator.Object);

        // Assert
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        response.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public async Task Should_Return204Ok_When_StudentsIsFound()
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

        Result<StudentResponse> result = new(student, true, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentByIdQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentAsync(mediator.Object, 1);

        // Assert
        result.Data.ShouldNotBeNull();
        response.ShouldBeOfType<Ok<Result<StudentResponse>>>();
    }

    [Fact]
    public async Task Should_Return404NotFound_When_StudentsIsFound()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentResponse student = null!;

        Result<StudentResponse> result = new(student, true, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<GetStudentByIdQuery>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.GetStudentAsync(mediator.Object, 2);

        // Assert
        result.Data.ShouldBeNull();
        response.ShouldBeOfType<NotFound<Result<StudentResponse>>>();
    }

    [Fact]
    public async Task Should_Return200Ok_When_PassedStudentIsValid()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentRequest student = new("Alice Johnson", "alice.johnson@example.com", "Computer Science", "Female");

        Result<bool> result = new(true, true, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<CreateStudentCommand>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.AddStudentAsync(mediator.Object, student);

        // Assert
        result.Data.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
        response.ShouldBeOfType<Ok<Result<bool>>>();
    }

    [Fact]
    public async Task Should_Return400BadRequest_When_PassedStudentIsNotValid()
    {
        // Arrange
        Mock<IMediator> mediator = new();
        StudentRequest student = new("Invalid Name", "invalid.email@example.com", "Computer Science", "Female");

        Result<bool> result = new(false, false, string.Empty);

        mediator
            .Setup(x => x.Send(It.IsAny<CreateStudentCommand>(), default))
            .ReturnsAsync(result);

        // Act
        IResult response = await StudentEndpoint.AddStudentAsync(mediator.Object, student);

        // Assert
        result.Data.ShouldBeFalse();
        result.IsSuccess.ShouldBeFalse();
        response.ShouldBeOfType<BadRequest<Result<bool>>>();
    }
}
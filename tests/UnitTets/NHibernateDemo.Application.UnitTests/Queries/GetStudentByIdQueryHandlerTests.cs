using Moq;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Application.Queries.Handlers;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;

namespace NHibernateDemo.Application.UnitTests.Queries;

public class GetStudentByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_ReturnSingleStudent_When_ProvidedIdIsFound()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Student student = new()
        {
            Id = 1,
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            Course = "Computer Science",
            Gender = "Female"
        };

        GetStudentByIdQuery query = new(1);
        Result<Student> repositoryResult = new(student, true, string.Empty);

        repository
            .Setup(x => x.GetStudentAsync(1))
            .ReturnsAsync(student);

        GetStudentByIdQueryHandler handler = new(repository.Object);

        // Act
        Result<StudentResponse> handlerResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldNotBeNull();

        handlerResult.Data.Id.ShouldBe(1);
        handlerResult.Data.Name.ShouldBe("Alice Johnson");
        handlerResult.Data.Email.ShouldBe("alice.johnson@example.com");
        handlerResult.Data.Course.ShouldBe("Computer Science");
        handlerResult.Data.Gender.ShouldBe("Female");

        handlerResult.IsSuccess.ShouldBeTrue();
        handlerResult.Message.ShouldBe(string.Empty);

        repository.Verify(x => x.GetStudentAsync(1), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailureMessage_When_ProvidedIdIsNotFound()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Student? student = null;

        GetStudentByIdQuery query = new(2);
        Result<Student> repositoryResult = new(student, true, string.Empty);

        repository
            .Setup(x => x.GetStudentAsync(2))
            .ReturnsAsync(student);

        GetStudentByIdQueryHandler handler = new(repository.Object);

        // Act
        Result<StudentResponse> handlerResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldBe(null);
        handlerResult.IsSuccess.ShouldBeFalse();
        handlerResult.Message.ShouldBe("Student with Id 2 not found!");

        repository.Verify(x => x.GetStudentAsync(2), Times.Once);
    }
}
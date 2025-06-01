using Moq;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Commands.Handlers;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;

namespace NHibernateDemo.Application.UnitTests.Commands;

public class CreateStudentCommandHandlerTests
{
    [Fact]
    public async Task Should_Return_ResultTrue_WhenSuccessfullyCreateStudent()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        StudentRequest studentRequest = new("Mike Schmidt", "mike.ms@mail.com", "Biology", "Male");
        Student student = studentRequest.ToDomain();

        CreateStudentCommand command = new(studentRequest);
        Result<bool> result = Result<bool>.Success(true);

        repository
            .Setup(x => x.AddStudentAsync(It.IsAny<Student>()))
            .ReturnsAsync(true);

        CreateStudentCommandHandler handler = new(repository.Object);

        // Act
        Result<bool> handlerResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldBeTrue();
        handlerResult.IsSuccess.ShouldBeTrue();
        handlerResult.Message.ShouldBe(string.Empty);

        repository.Verify(x => x.AddStudentAsync(It.IsAny<Student>()), Times.Once);
    }
}

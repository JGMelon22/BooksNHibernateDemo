using Moq;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Commands.Handlers;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;

namespace NHibernateDemo.Application.UnitTests.Commands;

public class RemoveStudentCommandHandlerTests
{
    [Fact]
    public async Task Should_Return_SuccessTrue_WhenStudentToBeRemovedIsFoundAndRemovedSuccessfully()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Student student = new()
        {
            Id = 2,
            Name = "Michael Thompson",
            Email = "michael.thompson@example.com",
            Course = "Mechanical Engineering",
            Gender = "Male"
        };

        RemoveStudentCommand command = new(2);
        Result<bool> repositoryResult = new(true, true, string.Empty);

        repository
            .Setup(x => x.GetStudentAsync(2))
            .ReturnsAsync(student);

        repository
            .Setup(x => x.RemoveStudentAsync(student.Id))
            .ReturnsAsync(true);

        RemoveStudentCommandHandler handler = new(repository.Object);

        // Act
        Result<bool> handlerResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldBeTrue();
        handlerResult.IsSuccess.ShouldBeTrue();
        handlerResult.Message.ShouldBe(string.Empty);

        repository.Verify(x => x.GetStudentAsync(2), Times.Once);
        repository.Verify(x => x.RemoveStudentAsync(2), Times.Once);
    }
}
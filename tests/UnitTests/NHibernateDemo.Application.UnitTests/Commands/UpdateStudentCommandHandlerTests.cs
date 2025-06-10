using Moq;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Commands.Handlers;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;
using ZiggyCreatures.Caching.Fusion;

namespace NHibernateDemo.Application.UnitTests.Commands;

public class UpdateStudentCommandHandlerTests
{
    [Fact]
    public async Task Should_Return_SuccessTrue_WhenStudentToUpdateIsFoundAndUpdatedSuccessfully()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Mock<IFusionCache> cache = new();

        StudentRequest studentRequest = new("Paul Madness", "paul.mdd@mail.com", "Geography", "Male");
        Student student = studentRequest.ToDomain();

        UpdateStudentCommand command = new(1, studentRequest);

        repository
                .Setup(x => x.GetStudentAsync(1))
                .ReturnsAsync(student);

        repository
            .Setup(x => x.UpdateStudentAsync(1, It.IsAny<Student>()))
            .ReturnsAsync(true);

        UpdateStudentCommandHandler handler = new(cache.Object, repository.Object);

        // Act
        Result<bool> handlerResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldBeTrue();
        handlerResult.IsSuccess.ShouldBeTrue();
        handlerResult.Message.ShouldBe(string.Empty);

        repository.Verify(x => x.GetStudentAsync(1), Times.Once);
        repository.Verify(x => x.UpdateStudentAsync(1, It.IsAny<Student>()), Times.Once);
    }
}

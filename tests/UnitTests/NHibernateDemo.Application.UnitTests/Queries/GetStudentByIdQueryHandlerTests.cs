using Moq;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Application.Queries.Handlers;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;
using ZiggyCreatures.Caching.Fusion;

namespace NHibernateDemo.Application.UnitTests.Queries;

public class GetStudentByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_ReturnSingleStudent_When_ProvidedIdIsFound()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Mock<IFusionCache> cache = new();

        Student student = new()
        {
            Id = 1,
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            Course = "Computer Science",
            Gender = "Female"
        };

        GetStudentByIdQuery query = new(1);

        cache
            .Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<FusionCacheFactoryExecutionContext<Student>, CancellationToken, Task<Student>>>(),
                default,
                null,
                null,
                CancellationToken.None
            ))
            .Returns(ValueTask.FromResult(student));

        repository
            .Setup(x => x.GetStudentAsync(1))
            .ReturnsAsync(student);

        GetStudentByIdQueryHandler handler = new(cache.Object, repository.Object);

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

        // repository.Verify(x => x.GetStudentAsync(1), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailureMessage_When_ProvidedIdIsNotFound()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        Mock<IFusionCache> cache = new();

        GetStudentByIdQuery query = new(2);

        cache
            .Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<FusionCacheFactoryExecutionContext<Student?>, CancellationToken, Task<Student?>>>(),
                default,
                null,
                null,
                CancellationToken.None
            ))
            .Returns(ValueTask.FromResult<Student?>(null));

        repository
            .Setup(x => x.GetStudentAsync(2))
            .ReturnsAsync((Student?)null);

        GetStudentByIdQueryHandler handler = new(cache.Object, repository.Object);

        // Act
        Result<StudentResponse> handlerResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldBe(null);
        handlerResult.IsSuccess.ShouldBeFalse();
        handlerResult.Message.ShouldBe("Student with Id 2 not found!");
    }
}
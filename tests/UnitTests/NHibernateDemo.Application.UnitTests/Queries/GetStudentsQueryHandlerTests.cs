using Moq;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Application.Queries.Handlers;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using Shouldly;

namespace NHibernateDemo.Application.UnitTests.Queries;

public class GetStudentsQueryHandlerTests
{
    [Fact]
    public async Task Should_ReturnStudentsCollection_When_ThereAreStudentsToBeReturned()
    {
        // Arrange
        Mock<IStudentRepository> repository = new();
        IEnumerable<Student> students =
        [
            new("Alice Johnson", "alice.johnson@example.com", "Computer Science", "Female"),
            new("Bob Smith", "bob.smith@example.com", "Mechanical Engineering", "Male"),
            new("Clara Lee", "clara.lee@example.com", "Electrical Engineering", "Female"),
            new("David Kim", "david.kim@example.com", "Business Administration", "Male"),
            new("Ella Martinez", "ella.martinez@example.com", "Psychology", "Female"),
        ];

        GetStudentsQuery query = new();

        repository
            .Setup(x => x.GetStudentsListAsync())
            .ReturnsAsync(students);

        GetStudentsQueryHandler handler = new(repository.Object);

        // Act
        Result<IEnumerable<StudentResponse>> handlerResult = await handler.Handle(query, CancellationToken.None);

        // Assert
        handlerResult.Data.ShouldNotBeEmpty();
        handlerResult.Data.Count().ShouldBe(5);
        handlerResult.IsSuccess.ShouldBeTrue();
        handlerResult.Message.ShouldBe(string.Empty);

        repository.Verify(x => x.GetStudentsListAsync(), Times.Once);
    }
}

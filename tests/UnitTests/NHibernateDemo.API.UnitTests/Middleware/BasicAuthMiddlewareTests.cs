using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NHibernateDemo.API.Middleware;
using NHibernateDemo.Infrastructure.Configuration;
using Shouldly;
using System.Text;

namespace NHibernateDemo.API.UnitTests.Middleware;

public class BasicAuthMiddlewareTests
{
    [Fact]
    public async Task Should_ReturnUnauthorized_When_NoCredentialProvided()
    {
        // Arrange
        Mock<ILogger<BasicAuthMiddleware>> logger = new();
        Mock<IOptions<BasicAuthOptions>> options = new();

        BasicAuthOptions basicAuth = new() { Username = "test-username", Password = "testpassword" };
        options.Setup(x => x.Value).Returns(basicAuth);
        RequestDelegate next = (HttpContext context) => Task.CompletedTask;

        DefaultHttpContext context = new();
        BasicAuthMiddleware middleware = new(next, logger.Object, options.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_InvalidBase64()
    {
        // Arrange
        Mock<ILogger<BasicAuthMiddleware>> logger = new();
        Mock<IOptions<BasicAuthOptions>> options = new();

        BasicAuthOptions basicAuth = new() { Username = "test-username", Password = "testpassword" };
        options.Setup(x => x.Value).Returns(basicAuth);
        RequestDelegate next = (HttpContext context) => Task.CompletedTask;

        DefaultHttpContext context = new();
        context.Request.Headers["Authorization"] = "Basic invalid-base64";
        BasicAuthMiddleware middleware = new(next, logger.Object, options.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Should_ReturnUnauthorized_When_WrongCredentials()
    {
        // Arrange
        Mock<ILogger<BasicAuthMiddleware>> logger = new();
        Mock<IOptions<BasicAuthOptions>> options = new();

        BasicAuthOptions basicAuth = new() { Username = "test-username", Password = "testpassword" };
        options.Setup(x => x.Value).Returns(basicAuth);
        RequestDelegate next = (HttpContext context) => Task.CompletedTask;

        DefaultHttpContext context = new();
        string wrongCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("wrong-user:wrong-pass"));
        context.Request.Headers["Authorization"] = $"Basic {wrongCredentials}";
        BasicAuthMiddleware middleware = new(next, logger.Object, options.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task Should_CallNext_WhenValidCredentials()
    {
        // Arrange
        Mock<ILogger<BasicAuthMiddleware>> logger = new();
        Mock<IOptions<BasicAuthOptions>> options = new();
        bool nextCalled = false;

        BasicAuthOptions basicAuth = new() { Username = "test-username", Password = "testpassword" };
        options.Setup(x => x.Value).Returns(basicAuth);
        RequestDelegate next = (HttpContext context) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        DefaultHttpContext context = new();
        string validCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("test-username:testpassword"));
        context.Request.Headers["Authorization"] = $"Basic {validCredentials}";
        BasicAuthMiddleware middleare = new(next, logger.Object, options.Object);

        // Act
        await middleare.InvokeAsync(context);

        // Assert
        nextCalled.ShouldBeTrue();
    }
}

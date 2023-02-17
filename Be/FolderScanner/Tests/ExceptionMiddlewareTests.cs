using FolderScanner.Middleware;
using Moq;
using FluentAssertions;
using Xunit;
using System.Text;

namespace FolderScanner.Tests;

public class ExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _next;
    private readonly Mock<ILogger<ExceptionMiddleware>> _logger;
    private readonly ExceptionMiddleware _subject;

    public ExceptionMiddlewareTests()
    {
        _next = new Mock<RequestDelegate>();
        _logger = new Mock<ILogger<ExceptionMiddleware>>();

        _subject = new ExceptionMiddleware(_next.Object, _logger.Object);
    }

    [Fact]
    public async Task InvokeAsync_Exception_LogsError()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _next.Setup(n => n(It.IsAny<HttpContext>())).Throws(exception);

        var context = new DefaultHttpContext();

        // Act
        await _subject.InvokeAsync(context).ConfigureAwait(false);

        // Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_NoException_NoLogError()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        await _subject.InvokeAsync(context).ConfigureAwait(false);

        // Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_Exception_SetsContextResponse()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _next.Setup(n => n(It.IsAny<HttpContext>())).Throws(exception);

        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        
        // Act
        await _subject.InvokeAsync(context).ConfigureAwait(false);


        // Assert
        context.Response.Body.Position = 0;
        using StreamReader streamReader = new(context.Response.Body);
        var actualResponseText = await streamReader.ReadToEndAsync();

        actualResponseText.Should().Be(exception.Message);
        context.Response.StatusCode.Should().Be(400);
        context.Response.ContentType.Should().Be("text/plain");
    }

    [Fact]
    public async Task InvokeAsync_NoException_ContextResponseUnchanged()
    {
        // Arrange
        const string body = "testing body";
        const int statusCode = 200;
        const string contentType = "application/json";

        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream(Encoding.ASCII.GetBytes(body)),
                StatusCode = statusCode,
                ContentType = contentType
            }
        };

        // Act
        await _subject.InvokeAsync(context).ConfigureAwait(false);


        // Assert
        context.Response.Body.Position = 0;
        using StreamReader streamReader = new(context.Response.Body);
        var actualResponseText = await streamReader.ReadToEndAsync();

        actualResponseText.Should().Be(body);
        context.Response.StatusCode.Should().Be(statusCode);
        context.Response.ContentType.Should().Be(contentType);
    }
}
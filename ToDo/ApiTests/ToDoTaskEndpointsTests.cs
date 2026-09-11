using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ToDo.Application.Services;

namespace ApiTests;

public class ToDoTaskEndpointsTests
{
    private readonly Mock<IToDoTaskService> _toDoTaskServiceMock;
    private readonly HttpContext _httpContext;
    private readonly ILoggerFactory _loggerFactory;

    public ToDoTaskEndpointsTests()
    {
        _toDoTaskServiceMock = new Mock<IToDoTaskService>();
        _httpContext = new DefaultHttpContext();
        _loggerFactory = new NullLoggerFactory();
    }

    [Fact]
    public void Example()
    {
        var act = true;

        Assert.True(act);
    }
}

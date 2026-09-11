using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ToDo.Api.Endpoints;
using ToDo.Application.Common.Dto;
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
    public async Task Get_WhenCalled_CallsService()
    {
        var id = 1;

        var act = await ToDoTaskEndpoints.Get(id, _toDoTaskServiceMock.Object, _loggerFactory, _httpContext);

        _toDoTaskServiceMock.Verify(x => x.Get(id), Times.Once);
    }

    [Fact]
    public async Task Get_HasNoToDoTask_ReturnsNotFound()
    {
        var act = await ToDoTaskEndpoints.Get(1, _toDoTaskServiceMock.Object, _loggerFactory, _httpContext);

        Assert.IsType<NotFound>(act.Result);
    }

    [Fact]
    public async Task Get_HasToDoTask_ReturnsOk()
    {
        var id = 1;

        _toDoTaskServiceMock.Setup(x => x.Get(id)).ReturnsAsync(new GetToDoTaskDto
        {
            Id = id,
            Task = "Write some unit tests",
            Created = DateTime.UtcNow
        });

        var act = await ToDoTaskEndpoints.Get(id, _toDoTaskServiceMock.Object, _loggerFactory, _httpContext);

        Assert.IsType<Ok<GetToDoTaskDto>>(act.Result);
    }

    [Fact]
    public async Task Get_HasToDoTask_ReturnsExpectedInstance()
    {
        var id = 1;
        var expectedResult = new GetToDoTaskDto
        {
            Id = id,
            Task = "Write some unit tests",
            Created = DateTime.UtcNow
        };

        _toDoTaskServiceMock.Setup(x => x.Get(id)).ReturnsAsync(expectedResult);

        var act = await ToDoTaskEndpoints.Get(id, _toDoTaskServiceMock.Object, _loggerFactory, _httpContext);

        Assert.Equal(expectedResult, ((Ok<GetToDoTaskDto>)act.Result).Value);
    }

    [Fact]
    public async Task Get_ThrowsException_ReturnsInternalServerError()
    {
        var id = 1;

        _toDoTaskServiceMock.Setup(x => x.Get(id)).ThrowsAsync(new Exception("Broken"));

        var act = await ToDoTaskEndpoints.Get(id, _toDoTaskServiceMock.Object, _loggerFactory, _httpContext);

        Assert.IsType<InternalServerError>(act.Result);
    }

    [Fact]
    public async Task GetListing_WhenCalled_CallsService()
    {
        var listing = new ToDoTaskListingDto
        {
            Page = 1,
            PageSize = 20
        } ;

        var act = await ToDoTaskEndpoints.GetListing(listing, _toDoTaskServiceMock.Object);

        _toDoTaskServiceMock.Verify(x => x.GetListing(listing), Times.Once);
    }

    [Theory]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(6, 3)]
    public async Task GetListing_PageGreaterThanTotalPages_ReturnsNotFound(int page, int totalPages)
    {
        var listing = new ToDoTaskListingDto
        {
            Page = page,
            PageSize = 20
        };

        _toDoTaskServiceMock.Setup(x => x.GetListing(listing)).ReturnsAsync(new ToDoTaskListingResultDto
        {
            Results = [],
            TotalPages = totalPages,
            TotalResults = 10
        });

        var act = await ToDoTaskEndpoints.GetListing(listing, _toDoTaskServiceMock.Object);

        Assert.IsType<NotFound>(act.Result);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    public async Task GetListing_PageLessOrEqualToTotalPages_ReturnsOk(int page, int totalPages)
    {
        var listing = new ToDoTaskListingDto
        {
            Page = page,
            PageSize = 20
        };

        _toDoTaskServiceMock.Setup(x => x.GetListing(listing)).ReturnsAsync(new ToDoTaskListingResultDto
        {
            Results = [],
            TotalPages = totalPages,
            TotalResults = 10
        });

        var act = await ToDoTaskEndpoints.GetListing(listing, _toDoTaskServiceMock.Object);

        Assert.IsType<Ok<ToDoTaskListingResultDto>>(act.Result);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    public async Task GetListing_PageLessOrEqualToTotalPages_ReturnsExpectedResult(int page, int totalPages)
    {
        var listing = new ToDoTaskListingDto
        {
            Page = page,
            PageSize = 20
        };

        var expectedResult = new ToDoTaskListingResultDto
        {
            Results = [],
            TotalPages = totalPages,
            TotalResults = 10
        };

        _toDoTaskServiceMock.Setup(x => x.GetListing(listing)).ReturnsAsync(expectedResult);

        var act = await ToDoTaskEndpoints.GetListing(listing, _toDoTaskServiceMock.Object);

        Assert.Equal(expectedResult, ((Ok<ToDoTaskListingResultDto>)act.Result).Value);
    }
}

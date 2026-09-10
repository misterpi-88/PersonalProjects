using Microsoft.Extensions.Logging;
using ToDo.Application.Common.Dto;
using ToDo.Application.Data.Repositories;

namespace ToDo.Application.Services.Implementations;

public class ToDoTaskService : IToDoTaskService
{
    private readonly IToDoTaskRepository _toDoTaskRepository;
    private readonly ILogger<ToDoTaskService> _logger;

    public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, ILogger<ToDoTaskService> logger)
    {
        _toDoTaskRepository = toDoTaskRepository;
        _logger = logger;
    }
    
    public async Task<GetToDoTaskDto?> Get(int id)
    {
        _logger.LogDebug("Running Get Method (id: {0})", id);
        
        return await _toDoTaskRepository.Get(id);
    }

    public async Task<ToDoTaskListingResultDto> GetListing(ToDoTaskListingDto listing)
    {
        return await _toDoTaskRepository.GetListing(listing);
    }

    public async Task Create(CreateToDoTaskDto createToDoTask)
    {
        await _toDoTaskRepository.Create(createToDoTask);
    }

    public async Task<bool> Update(int id, UpdateToDoTaskDto updateToDoTask)
    {
        return await _toDoTaskRepository.Update(id, updateToDoTask);
    }

    public async Task<bool> Delete(int id)
    {
        return await _toDoTaskRepository.Delete(id);
    }
}

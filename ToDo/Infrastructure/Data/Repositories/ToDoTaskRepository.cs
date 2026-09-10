using ToDo.Application.Common.Dto;
using ToDo.Application.Data.Repositories;
using ToDo.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Infrastructure.Data.Repositories;

public class ToDoTaskRepository : IToDoTaskRepository
{
    private readonly ToDoDbContext _dbContext;

    public ToDoTaskRepository(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetToDoTaskDto?> Get(int id)
    {
        return await _dbContext.ToDoTasks
            .Where(x => x.Id == id && !x.Deleted.HasValue)
            .Select(x => new GetToDoTaskDto
            {
                Id = x.Id,
                Task = x.Task,
                Created = x.Created
            }).SingleOrDefaultAsync();
    }

    public async Task<ToDoTaskListingResultDto> GetListing(ToDoTaskListingDto listing)
    {
        var query = _dbContext.ToDoTasks
            .Where(x => !x.Deleted.HasValue);

        var result = new ToDoTaskListingResultDto
        {
            TotalResults = await query.CountAsync()
        };

        result.TotalPages = result.TotalResults > 0 ? (int)Math.Ceiling((decimal)result.TotalResults / listing.PageSize) : 0;

        if(result.TotalResults >= 1 && listing.Page <= result.TotalPages)
        {
            result.Results = await query
                .Skip(listing.Page * listing.PageSize - listing.PageSize)
                .Take(listing.PageSize)
                .Select(x => new GetToDoTaskDto
                {
                    Id = x.Id,
                    Task = x.Task,
                    Created = x.Created
                }).ToListAsync();
        }

        return result;
    }

    public async Task Create(CreateToDoTaskDto createToDoTask)
    {
        await _dbContext.ToDoTasks.AddAsync(new ToDoTask
        {
            Task = createToDoTask.Task,
            Created = DateTime.UtcNow
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> Update(int id, UpdateToDoTaskDto updateToDoTask)
    {
        var entity = await _dbContext.ToDoTasks.SingleOrDefaultAsync(x => x.Id == id && !x.Deleted.HasValue);

        if (entity == null)
        {
            return false;
        }

        entity.Task = updateToDoTask.Task;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _dbContext.ToDoTasks.SingleOrDefaultAsync(x => x.Id == id && !x.Deleted.HasValue);

        if (entity == null)
        {
            return false;
        }

        entity.Deleted = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

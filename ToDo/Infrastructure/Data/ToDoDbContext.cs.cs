using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Data.Entities;

namespace ToDo.Infrastructure.Data;

public class ToDoDbContext : DbContext
{
    public required DbSet<ToDoTask> ToDoTasks { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoDbContext).Assembly);
    }
}

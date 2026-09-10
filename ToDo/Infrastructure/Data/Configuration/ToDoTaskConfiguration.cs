using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Infrastructure.Data.Entities;

namespace ToDo.Infrastructure.Data.Configuration;

public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Task).HasMaxLength(1000);
        builder.ToTable("ToDoTasks");
        builder.Property(x => x.Created).IsRequired();
    }
}

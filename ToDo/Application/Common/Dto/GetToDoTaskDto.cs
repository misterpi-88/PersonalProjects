namespace ToDo.Application.Common.Dto;

public class GetToDoTaskDto
{
    public int Id { get; set; }
    public string Task { get; set; } = string.Empty;
    public DateTime Created { get; set; }
}

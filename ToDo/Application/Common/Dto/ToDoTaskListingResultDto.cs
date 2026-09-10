namespace ToDo.Application.Common.Dto;

public class ToDoTaskListingResultDto
{
    public IList<GetToDoTaskDto> Results { get; set; } = [];

    public int TotalPages { get; set; }

    public int TotalResults { get; set; }
}

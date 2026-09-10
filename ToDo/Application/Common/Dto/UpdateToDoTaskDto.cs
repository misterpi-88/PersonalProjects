using System.ComponentModel.DataAnnotations;

namespace ToDo.Application.Common.Dto;

public class UpdateToDoTaskDto
{
    [Required]
    [MinLength(50)]
    [MaxLength(1000)]
    public string Task { get; set; } = string.Empty;
}

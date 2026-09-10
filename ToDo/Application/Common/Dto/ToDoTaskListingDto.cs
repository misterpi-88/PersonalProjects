using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Application.Common.Dto;

public class ToDoTaskListingDto
{
    [Required]
    [FromRoute(Name = "p")]
    [Range(1, int.MaxValue)]
    public int Page { get; set; }

    [Required]
    [FromQuery(Name = "s")]
    [AllowedValues(10, 20)]
    public int PageSize { get; set; }
}

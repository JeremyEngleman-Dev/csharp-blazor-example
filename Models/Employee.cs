using System.ComponentModel.DataAnnotations;

namespace BlazorIntro.Models;

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
}

public class FormEmployee
{
    [Required(ErrorMessage = "Name is required")]
    public required string? Name { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(18, 90, ErrorMessage = "Age between 18 and 90")]
    public int? Age { get; set; }

    public bool Status { get; set; }
}
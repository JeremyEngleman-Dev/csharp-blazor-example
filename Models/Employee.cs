namespace BlazorIntro.Models;

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
}
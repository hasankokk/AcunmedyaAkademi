using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models;

[Index(nameof(StudentTckn), IsUnique = true)]
public class Student
{
    public int StudentId { get; set; }
    [MaxLength(11)]
    public long StudentTckn { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Surname { get; set; }
    [MaxLength(150)]
    public ICollection<Classroom>? Classrooms { get; set; } = new List<Classroom>();
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models;

[Index(nameof(TeacherTckn), IsUnique = true)]
public class Teacher
{
    public int TeacherId { get; set; }
    [MaxLength(11)]
    public long TeacherTckn { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Surname { get; set; }
    
    public ICollection<Classroom>? Classrooms { get; set; } =  new List<Classroom>();
}
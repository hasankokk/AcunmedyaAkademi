namespace ClassroomManagement.Models;

public class Teachers
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public List<Classrooms> Classrooms { get; set; } = new List<Classrooms>();
}
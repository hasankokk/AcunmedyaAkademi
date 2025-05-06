namespace ClassroomManagement.Models;

public class Students
{
    public int No {  get; set; }
    public string? Name  { get; set; }
    public string? LastName { get; set; }
    public List<Classrooms> Classrooms { get; set; } = new List<Classrooms>();
}
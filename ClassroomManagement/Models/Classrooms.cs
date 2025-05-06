namespace ClassroomManagement.Models;

public class Classrooms
{
    public string? Name { get; set; }
    public List<Students> Students { get; set; } =  new List<Students>();
    public List<Teachers> Teachers { get; set; } =  new List<Teachers>();
}
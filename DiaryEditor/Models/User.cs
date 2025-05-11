namespace DiaryEditor.Models;

public class User
{
    public int Id {get; set;}
    public DateTime Created { get; set; } = DateTime.Now;
    public string Username {get; set;}
    public string? FirstName { get; set;}
    public string? LastName {get; set;}
    public string? Password {get; set;}
    public DateOnly BirthDate {get; set;}
    public string? Gender {get; set;}
    public string? SecurityQuestion {get; set;}
    public string? SecurityAnswer {get; set;}
    public string? Email {get; set;}
    
    public bool IsActive {get; set;}
}


using Microsoft.EntityFrameworkCore;

namespace ConsoleChatApp.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string SecurityQuestion { get; set; }
    public string SecurityAnswer { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now; // varsayılan değer
}
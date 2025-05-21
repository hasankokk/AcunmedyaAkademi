using ConsoleChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleChatApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=yourservername;Database=yourdatabasename;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=StudentManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        //or "Server=yourservername;Database=yourdatabasename;User Id=yourusername;Password=yourpassword;Trusted_Connection=False;TrustServerCertificate=True;"
    }
}
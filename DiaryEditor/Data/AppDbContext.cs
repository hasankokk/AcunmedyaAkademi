using DiaryEditor.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryEditor.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Daily> Dailies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=yourservername;Database=yourdatabasename;Trusted_Connection=True;TrustServerCertificate=True;");
        //or "Server=yourservername;Database=yourdatabasename;User Id=yourusername;Password=yourpassword;Trusted_Connection=False;TrustServerCertificate=True;"
    }
}
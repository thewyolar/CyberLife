using CyberLife.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberLife.DAO;

public class DAOA : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Cyber;Username=postgres;Password=postgres");
    }
    
}
using CyberLife.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberLife.Data;

public class ApplicationDbContext :  IdentityDbContext<User, IdentityRole, string>
{
    
    public DbSet<MapModel> maps { get; set; } = null!;
    public DbSet<PerceptronModel> perceptrons { get; set; } = null!;
    // public DbSet<LayerModel> layers { get; set; } = null!;
    public DbSet<Admin> admins { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
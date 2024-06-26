﻿using CyberLife.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberLife.Data;

public class ApplicationDbContext :  IdentityDbContext<User, IdentityRole, string>
{
    
    public DbSet<MapModel> Maps { get; set; } = null!;
    public DbSet<PerceptronModel> Perceptrons { get; set; } = null!;
    public DbSet<LayerModel> Layers { get; set; } = null!;

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
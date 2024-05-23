using System.Net;
using CyberLife.Models;
using Microsoft.AspNetCore.Identity;

namespace CyberLife.Data;

public class RoleInitializer
{
    private static readonly ILogger<RoleInitializer> _logger;

    static RoleInitializer()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        _logger = loggerFactory.CreateLogger<RoleInitializer>();
    }

    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminLogin = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
        string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        _logger.LogInformation("Admin login: {AdminLogin}, Admin password: {AdminPassword}", adminLogin, adminPassword);

        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            _logger.LogInformation("Admin role created.");
        }
        if (await roleManager.FindByNameAsync("User") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
            _logger.LogInformation("User role created.");
        }

        if (adminLogin != null && adminPassword != null)
        {
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { Email = adminLogin, UserName = adminLogin, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    _logger.LogInformation("Admin user created and added to Admin role.");
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Domain;

namespace API.Data;

public static class Seed
{
    public static async Task SeedUsersAndRolesAsync(IServiceProvider services)
    {
        // Ensure database is created and up-to-date
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Database migration failed: " + ex.Message);
            throw;
        }

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = new[] { "Sales", "User", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        string adminEmail = "admin@et.vn";
        string adminPassword = "P@ssw0rd"; // You can change this default password
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Sales");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Ensure user is in Sales role
            if (!await userManager.IsInRoleAsync(adminUser, "Sales"))
            {
                await userManager.AddToRoleAsync(adminUser, "Sales");
            }
            // Ensure user is in Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        // Add user@et.vn with User role
        string normalUserEmail = "user@et.vn";
        string normalUserPassword = "P@ssw0rd"; // You can change this default password
        var normalUser = await userManager.FindByEmailAsync(normalUserEmail);
        if (normalUser == null)
        {
            normalUser = new ApplicationUser
            {
                UserName = normalUserEmail,
                Email = normalUserEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(normalUser, normalUserPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }
        else
        {
            // Ensure user is in User role
            if (!await userManager.IsInRoleAsync(normalUser, "User"))
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }
    }
} 
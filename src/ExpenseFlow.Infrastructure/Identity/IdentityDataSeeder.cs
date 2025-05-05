using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Infrastructure.Identity;
public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = new[] { "Admin", "Personnel" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        //default admin 
        var adminEmail = "admin@expenseflow.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName    = "System Administrator", 
                EmailConfirmed = true,
                CreatedAt   = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // default personnel 
        var personnelEmail = "personnel@expenseflow.com";
        if (await userManager.FindByEmailAsync(personnelEmail) == null)
        {
            var personnel = new ApplicationUser
            {
                UserName = personnelEmail,
                Email = personnelEmail,
                FullName    = "Default Personnel",
                EmailConfirmed = true,
                CreatedAt   = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(personnel, "Personnel123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(personnel, "Personnel");
        }

    }
}

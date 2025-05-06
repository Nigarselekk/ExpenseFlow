using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Infrastructure.Identity
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ExpenseFlowDbContext>();

            //Roles
            string[] roles = new[] { "Admin", "Personnel" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Default admin
            var adminEmail = "admin@expenseflow.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Default personnel
            var personnelEmail = "personnel@expenseflow.com";
            var personnelUser = await userManager.FindByEmailAsync(personnelEmail);
            if (personnelUser == null)
            {
                personnelUser = new ApplicationUser
                {
                    UserName = personnelEmail,
                    Email = personnelEmail,
                    FullName = "Default Personnel",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(personnelUser, "Personnel123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(personnelUser, "Personnel");
            }

            //Upsert
            var existing = await dbContext.Personnels
                .SingleOrDefaultAsync(p => p.ApplicationUserId == personnelUser.Id);

            var newId = Guid.Parse(personnelUser.Id);
            if (existing == null)
            {

                var personnelEntity = new Personnel
                {
                    Id = newId,
                    ApplicationUserId = personnelUser.Id,
                    FirstName = "Default",
                    LastName = "Personnel",
                    Email = personnelEmail,
                    PhoneNumber = "05555555555",
                    Address = "Default Address",
                    City = "Default City",
                };
                dbContext.Personnels.Add(personnelEntity);
            }
            else if (existing.Id != newId)
            {

                existing.Id = newId;
                dbContext.Personnels.Update(existing);
            }
            if (!await dbContext.ExpenseCategories.AnyAsync())
            {
                var defaultCategories = new[]
                {
                    new ExpenseCategory { Name = "Yakıt",           Description = "Araç yakıt masrafları" },
                    new ExpenseCategory { Name = "Konaklama",      Description = "Otel masrafı" },
                    new ExpenseCategory { Name = "Barınma",         Description = "Barınma giderleri; otel, apart vs" },
                    new ExpenseCategory { Name = "Yemek Giderleri", Description = "Günlük sadece 3 öğün karşılanacaktır." }
                };
                dbContext.ExpenseCategories.AddRange(defaultCategories);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRoleAsync(roleManager, "Admin");
        await SeedRoleAsync(roleManager, "User");

        await SeedUserAsync(userManager, "admin", "Admin123!", "Admin");
        await SeedUserAsync(userManager, "user", "User123!", "User");
    }

    private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string role)
    {
        if (await roleManager.RoleExistsAsync(role))
        {
            return;
        }

        var result = await roleManager.CreateAsync(new IdentityRole(role));

        EnsureSucceeded(result);
    }

    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string userName, string password, string role)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user is not null)
        {
            return;
        }

        user = new ApplicationUser
        {
            UserName = userName
        };

        var result = await userManager.CreateAsync(user, password);

        EnsureSucceeded(result);

        var roleResult = await userManager.AddToRoleAsync(user, role);

        EnsureSucceeded(roleResult);
    }

    private static void EnsureSucceeded(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ", result.Errors.Select(x => x.Description)));
        }
    }
}
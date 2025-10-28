using Microsoft.AspNetCore.Identity;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> users, RoleManager<AppRole> roles)
        {
            var roleNames = new[] { "User", "Admin" };
            foreach (var r in roleNames)
            {
                if (!await roles.RoleExistsAsync(r))
                {
                    var role = new AppRole { Name = r, NormalizedName = r.ToUpperInvariant() };
                    var rRes = await roles.CreateAsync(role);
                    if (!rRes.Succeeded)
                        throw new InvalidOperationException($"Nie udało się utworzyć roli '{r}': {string.Join(", ", rRes.Errors.Select(e => e.Description))}");
                }
            }

            const string demoEmail = "demo@tripsplit.app";
            const string demoPass = "Demo!123";

            var demo = await users.FindByEmailAsync(demoEmail);
            if (demo is null)
            {
                demo = new AppUser
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    EmailConfirmed = true,
                    FirstName = "Demo",
                    LastName = "User"
                };

                var createRes = await users.CreateAsync(demo, demoPass);
                if (!createRes.Succeeded)
                    throw new InvalidOperationException($"Nie udało się utworzyć konta demo: {string.Join(", ", createRes.Errors.Select(e => e.Description))}");
            }

            if (!await users.IsInRoleAsync(demo, "User"))
            {
                var addRoleRes = await users.AddToRoleAsync(demo, "User");
                if (!addRoleRes.Succeeded)
                    throw new InvalidOperationException($"Nie udało się dodać roli 'User' do demo: {string.Join(", ", addRoleRes.Errors.Select(e => e.Description))}");
            }

            const string adminEmail = "admin@tripsplit.app";
            const string adminPass = "Admin!123";

            var admin = await users.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "TripSplit",
                    LastName = "Admin"
                };

                var createAdminRes = await users.CreateAsync(admin, adminPass);
                if (!createAdminRes.Succeeded)
                    throw new InvalidOperationException($"Nie udało się utworzyć konta admin: {string.Join(", ", createAdminRes.Errors.Select(e => e.Description))}");
            }

            if (!await users.IsInRoleAsync(admin, "Admin"))
            {
                var addAdminRoleRes = await users.AddToRoleAsync(admin, "Admin");
                if (!addAdminRoleRes.Succeeded)
                    throw new InvalidOperationException($"Nie udało się dodać roli 'Admin' do admina: {string.Join(", ", addAdminRoleRes.Errors.Select(e => e.Description))}");
            }
        }
    }
}


namespace TaskTracker.Server.Data
{
    public static class DBSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Get required services
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserModel>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created and migrations are applied
            context.Database.Migrate();

            // Seed Roles
            await SeedRolesAsync(roleManager);

            // Seed Admin User
            await SeedAdminUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Define roles to be created
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Check if role exists
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // Create role
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine($"Role '{roleName}' created successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create role '{roleName}'.");
                        foreach (var error in roleResult.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<UserModel> userManager)
        {
            // Check if the admin user already exists
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                // Create admin user
                adminUser = new UserModel
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    IsAdmin = true,
                    EmailConfirmed = true // Optional: Set to true if you require email confirmation
                };

                // Create the user with a default password
                var result = await userManager.CreateAsync(adminUser, "Password123!");

                if (result.Succeeded)
                {
                    // Add user to Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user.");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
    }
}

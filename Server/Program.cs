
var builder = WebApplication.CreateBuilder(args);

// Configure services
var services = builder.Services;
var configuration = builder.Configuration;
IHostEnvironment env = builder.Environment;

// Configure DbContext with SQL Server connection
services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Register services
services.AddScoped<ITaskService, TaskService>();
services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<INotificationService, NotificationService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IUserService, UserService>();

// Add Identity services and configure password settings
services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options if needed
services.Configure<IdentityOptions>(options =>
{
    // Password settings, lockout settings, etc.
});

// Get JWT key from configuration
var jwtKey = configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT Key is missing. Please add it to your configuration.");
}

// Configure JWT Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configure CORS to allow requests from Blazor client
services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://localhost:5001", "http://localhost:5000", "https://mytasktrackerpro.site:443")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Add controllers and views
services.AddControllersWithViews();
services.AddRazorPages();

// Build the application
var app = builder.Build();

// Apply migrations and run the seeding logic
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<UserModel>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        await dbContext.Database.MigrateAsync();

        await DBSeed.SeedAsync(serviceProvider);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during migration or seeding: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Serve Blazor WebAssembly framework files
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// Configure routing
app.UseRouting();

// Enable CORS for cross-origin requests
app.UseCors("AllowAll");

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages and Controllers
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Listen on port 443 (inside the container) if the environment is Production
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://+:443";
    app.Urls.Add(port); // Set the application to listen on the specified URL (default to port 443)
}

// Start the application
app.Run();

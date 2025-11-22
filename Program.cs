using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        
        // options.Cookie.HttpOnly = true;
        // options.Cookie.SameSite = SameSiteMode.Lax;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;


        // Configure cookie settings
        // Note: Cookie authentication is shared across tabs in the same browser
        // To login with different accounts, use:
        // 1. Different browsers (Chrome, Firefox, Edge)
        // 2. Incognito/Private mode for one tab
        // 3. Different browser profiles
    });

builder.Services.AddAuthorization();

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Check if database can connect
        if (!context.Database.CanConnect())
        {
            // Create database and tables if database doesn't exist
            context.Database.EnsureCreated();
        }
        else
        {
            // Check if tables exist by trying to query SystemRoles
            try
            {
                var _ = context.SystemRoles.Any();
            }
            catch
            {
                // Tables don't exist, create them
                context.Database.EnsureCreated();
            }
        }
        
        // Seed SystemRoles if they don't exist
        if (!context.SystemRoles.Any())
        {
            context.SystemRoles.AddRange(
                new FinalASB.Models.SystemRole { RoleName = "Admin" },
                new FinalASB.Models.SystemRole { RoleName = "User" }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log error if needed
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB: {Message}", ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


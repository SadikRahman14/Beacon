using Beacon.Data;
using Beacon.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// ADD THIS:
using Beacon.Data.Seeders; // <-- seeder namespace

var builder = WebApplication.CreateBuilder(args);

// --- DB ---
var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(connectionString));

// --- Identity with Roles enabled ---
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        // options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Cookie paths
builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login";
    o.LogoutPath = "/Account/Logout";
    o.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

// --- External auth (Google) ---
builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

var app = builder.Build();

//
// ---------- DB migrate + CSV seed (NEW) ----------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync(); // ensure DB/tables exist

    var env = services.GetRequiredService<IWebHostEnvironment>();
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Seeder");
    var csvPath = Path.Combine(env.WebRootPath ?? "wwwroot", "data", "dhaka_location_counts.csv");

    await LocationCountsSeeder.SeedAsync(app.Services, csvPath, logger);
}
// ---------- End migrate + seed ----------

// ---------- Seed Admin (role + user) ----------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    const string adminRole = "admin";

    var adminEmail = builder.Configuration["Admin:Email"] ?? "admin@beacon.local";
    var adminPassword = builder.Configuration["Admin:Password"] ?? "Admin#12345";

    // 1) Ensure role exists
    if (!await roleManager.RoleExistsAsync(adminRole))
        await roleManager.CreateAsync(new IdentityRole(adminRole));

    // 2) Ensure user exists
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var create = await userManager.CreateAsync(adminUser, adminPassword);
        if (!create.Succeeded)
            throw new Exception("Failed to create admin user: " +
                                string.Join(", ", create.Errors.Select(e => e.Description)));
    }

    // 3) Ensure user is in role
    if (!await userManager.IsInRoleAsync(adminUser, adminRole))
    {
        var add = await userManager.AddToRoleAsync(adminUser, adminRole);
        if (!add.Succeeded)
            throw new Exception("Failed to add admin role: " +
                                string.Join(", ", add.Errors.Select(e => e.Description)));
    }
}
// ---------- End seeding ----------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

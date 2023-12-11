using HospitalProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

builder.Services.AddIdentity<CustomUser, IdentityRole>(options =>
{
    // Configure Identity options
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always; // Set HttpOnly to enhance security
    options.Secure = CookieSecurePolicy.Always; // Set to Always when using HTTPS
});
builder.Services.AddCors(options =>         // Handling Cross-Origin Requests
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
var userManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();


var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

// Ensure roles exist
await EnsureRoleExists(roleManager, "Admin");
await EnsureRoleExists(roleManager, "Patient");
await EnsureRoleExists(roleManager, "Doctor");
await EnsureRoleExists(roleManager, "Scheduler");

var adminEmail = "admin@example.com";

var adminUser = new CustomUser { UserName = adminEmail, Email = adminEmail, Password = "admin" };

// CreateAsync will also add the user to the database if it doesn't exist
var result = await userManager.CreateAsync(adminUser, "admin");

if (result.Succeeded)
{
    await userManager.AddToRoleAsync(adminUser, "Admin");
}
else
{
    Console.WriteLine("Error while creating the user: admin");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseCors();

app.MapControllers();

app.Run();

async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
{
    var roleExists = await roleManager.RoleExistsAsync(roleName);
    if (!roleExists)
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}

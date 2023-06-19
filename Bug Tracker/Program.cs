using Bug_Tracker.Data;
using Bug_Tracker.Infrastructure;
using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IWrapperRepository, WrapperRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordValidator<Employee>, CustomPasswordValidator>();
builder.Services.AddScoped<IUserValidator<Employee>, CustomUserValidator>();


builder.Services.AddIdentity<Employee, IdentityRole>(opt =>
{

    opt.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//app.MapControllerRoute(
//    name: "userlisting",
//    pattern: "{controller=AdminUser}/{action=Index}/{page?}");

app.MapControllerRoute(
    name: "homepage",
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


SeedData.EnsurePopulated(app);
SeedData.CreateAdminUser(app);

app.Run();

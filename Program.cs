using Microsoft.EntityFrameworkCore;
using Fucam.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Database kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình xác thực (Authentication)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/quan-tri/login";
        options.AccessDeniedPath = "/quan-tri/login";
    });

var app = builder.Build();

// Tự động tạo Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Tạo sẵn tài khoản admin mặc định nếu chưa có
    if (!context.AdminUsers.Any())
    {
        var hasher = new PasswordHasher<AdminUser>();
        var admin = new AdminUser { Username = "admin" };
        admin.PasswordHash = hasher.HashPassword(admin, "admin"); // Mật khẩu mặc định: admin
        context.AdminUsers.Add(admin);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

using Microsoft.EntityFrameworkCore;
using Fucam.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

// Cấu hình Serilog - ghi log ra Console + File
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,       // Mỗi ngày 1 file log mới
        retainedFileCountLimit: 30,                 // Giữ lại tối đa 30 ngày
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

try
{
    Log.Information("Ứng dụng Fucamp đang khởi động...");

    var builder = WebApplication.CreateBuilder(args);

    // Sử dụng Serilog thay thế logging mặc định
    builder.Host.UseSerilog();

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
    // using (var scope = app.Services.CreateScope())
    // {
    //     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //     context.Database.EnsureCreated();

    //     var admin = context.AdminUsers.FirstOrDefault(u => u.Username == "admin");
    //     if (admin == null)
    //     {
    //         admin = new AdminUser { Username = "admin" };
    //         admin.PasswordHash = Fucam.Helpers.HashHelper.ComputeMD5("admin"); // Mật khẩu mặc định: admin
    //         context.AdminUsers.Add(admin);
    //         context.SaveChanges();
    //     }
    //     else if (admin.PasswordHash.Length != 32)
    //     {
    //         // Chuyển đổi hash cũ (Identity) sang MD5
    //         admin.PasswordHash = Fucam.Helpers.HashHelper.ComputeMD5("admin");
    //         context.SaveChanges();
    //     }
    // }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();   // Static files TRƯỚC UseRouting (best practice)
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    // Ghi log mỗi request HTTP (tuỳ chọn, rất hữu ích để debug production)
    app.UseSerilogRequestLogging();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Log.Information("Ứng dụng Fucamp đã khởi động thành công!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng Fucamp bị lỗi nghiêm trọng và không thể khởi động!");
}
finally
{
    Log.CloseAndFlush();
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fucam.Models;

namespace Fucam.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

        [HttpPost]
        public IActionResult Register(Registration model)
        {
            if (ModelState.IsValid)
            {
                // Tránh lỗi DB nếu cột đang bị Set NOT NULL trong SQL Server
                model.PhoneNumber = model.PhoneNumber ?? "";
                model.Email = model.Email ?? "";

                _context.Registrations.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Đăng ký tham gia thành công! Chúng tôi sẽ liên hệ lại với bạn sớm nhất.";
                return RedirectToAction(nameof(Index));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Error"] = string.Join("<br/>", errors);
            return RedirectToAction(nameof(Index));
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fucam.Models;

namespace Fucam.Controllers
{
    [Route("quan-tri")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Index(string searchString, string dateFrom, string dateTo, int? statusFilter, int page = 1)
        {
            // === THỐNG KÊ QUAN TRỌNG (CHART) ===
            var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-i)).Reverse().ToList();
            var chartLabels = new List<string>();
            var chartCounts = new List<int>();

            foreach (var day in last7Days)
            {
                chartLabels.Add(day.ToString("dd/MM"));
                chartCounts.Add(_context.Registrations.Count(r => r.CreatedAt.Date == day));
            }

            ViewBag.ChartLabels = string.Join(",", chartLabels.Select(l => $"'{l}'")); 
            ViewBag.ChartCounts = string.Join(",", chartCounts);
            ViewBag.GlobalTotal = _context.Registrations.Count();
            ViewBag.TodayTotal = _context.Registrations.Count(r => r.CreatedAt.Date == DateTime.Today);
            ViewBag.ContactedTotal = _context.Registrations.Count(r => r.Status == 1);
            ViewBag.NotContactedTotal = _context.Registrations.Count(r => r.Status == 0);
            // ===================================

            int pageSize = 10;
            var query = _context.Registrations.AsQueryable();

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();
                query = query.Where(r => 
                    (r.ParentName != null && r.ParentName.ToLower().Contains(search)) || 
                    (r.PhoneNumber != null && r.PhoneNumber.Contains(search)) || 
                    (r.Email != null && r.Email.ToLower().Contains(search)) || 
                    (r.ChildInfo != null && r.ChildInfo.ToLower().Contains(search)));
            }

            // Lọc theo ngày bắt đầu
            if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var fromDate))
            {
                query = query.Where(r => r.CreatedAt.Date >= fromDate.Date);
            }

            // Lọc theo ngày kết thúc
            if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var toDate))
            {
                query = query.Where(r => r.CreatedAt.Date <= toDate.Date);
            }

            // Lọc theo trạng thái
            if (statusFilter.HasValue && statusFilter.Value >= 0)
            {
                query = query.Where(r => r.Status == statusFilter.Value);
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var registrations = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.TotalRecords = totalRecords;

            return View(registrations);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.AdminUsers.FirstOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    if (user.PasswordHash == Fucam.Helpers.HashHelper.ComputeMD5(model.Password))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index");
                    }
                }
                TempData["Error"] = "Tài khoản hoặc mật khẩu không chính xác.";
            }
            else
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
            }
            return RedirectToAction("Login");
        }

        public class ToggleStatusRequest
        {
            public string? Note { get; set; }
        }

        [HttpPost("toggle-status/{id}")]
        [Authorize]
        public IActionResult ToggleStatus(int id, [FromBody] ToggleStatusRequest req)
        {
            var reg = _context.Registrations.Find(id);
            if (reg == null)
                return Json(new { success = false, message = "Không tìm thấy bản ghi." });

            if (reg.Status == 1)
                return Json(new { success = false, message = "Bản ghi đã được đánh dấu liên hệ, không thể hoàn tác." });

            reg.Status = 1;
            
            if (req != null && !string.IsNullOrWhiteSpace(req.Note))
            {
                reg.Notes = req.Note;
            }
            
            _context.SaveChanges();
            return Json(new { success = true, newStatus = reg.Status, newNote = reg.Notes });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}

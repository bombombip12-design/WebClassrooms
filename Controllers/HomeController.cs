using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using FinalASB.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalASB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var classes = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Owner)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Enrollments)
                .Select(e => new ClassViewModel
                {
                    Id = e.Class.Id,
                    ClassName = e.Class.ClassName,
                    Description = e.Class.Description,
                    JoinCode = e.Class.JoinCode,
                    CoverImageUrl = e.Class.CoverImageUrl,
                    OwnerName = e.Class.Owner.FullName,
                    StudentCount = e.Class.Enrollments.Count,
                    UserRole = e.Role,
                    CreatedAt = e.Class.CreatedAt
                })
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(classes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}


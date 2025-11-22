using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using FinalASB.Models;
using FinalASB.ViewModels;
using System.Security.Claims;

namespace FinalASB.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var joinCode = GenerateJoinCode();

            var newClass = new Class
            {
                ClassName = model.ClassName,
                Description = model.Description,
                JoinCode = joinCode,
                OwnerId = userId,
                CreatedAt = DateTime.Now
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            // Auto-enroll the creator as Teacher
            var enrollment = new Enrollment
            {
                UserId = userId,
                ClassId = newClass.Id,
                Role = "Teacher",
                JoinedAt = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = newClass.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var enrollment = await _context.Enrollments
                .Include(e => e.Class)
                    .ThenInclude(c => c.Owner)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Enrollments)
                        .ThenInclude(en => en.User)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Assignments)
                        .ThenInclude(a => a.Creator)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Announcements)
                        .ThenInclude(an => an.User)
                .FirstOrDefaultAsync(e => e.ClassId == id && e.UserId == userId);

            if (enrollment == null)
            {
                return NotFound();
            }

            ViewBag.UserRole = enrollment.Role;
            ViewBag.IsOwner = enrollment.Class.OwnerId == userId;

            return View(enrollment.Class);
        }

        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(string joinCode)
        {
            if (string.IsNullOrWhiteSpace(joinCode))
            {
                ModelState.AddModelError("", "Mã lớp học không được để trống.");
                return View();
            }

            var classEntity = await _context.Classes
                .FirstOrDefaultAsync(c => c.JoinCode == joinCode);

            if (classEntity == null)
            {
                ModelState.AddModelError("", "Mã lớp học không hợp lệ.");
                return View();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Check if already enrolled
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == classEntity.Id);

            if (existingEnrollment != null)
            {
                return RedirectToAction("Details", new { id = classEntity.Id });
            }

            var enrollment = new Enrollment
            {
                UserId = userId,
                ClassId = classEntity.Id,
                Role = "Student",
                JoinedAt = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = classEntity.Id });
        }

        private string GenerateJoinCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string code;
            bool isUnique;

            do
            {
                code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                isUnique = !_context.Classes.Any(c => c.JoinCode == code);
            } while (!isUnique);

            return code;
        }
    }
}


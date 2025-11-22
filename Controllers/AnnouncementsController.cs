using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using FinalASB.Models;
using System.Security.Claims;

namespace FinalASB.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int classId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Nội dung thông báo không được để trống.";
                return RedirectToAction("Details", "Classes", new { id = classId });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == classId);

            if (enrollment == null)
            {
                return Forbid();
            }

            var announcement = new Announcement
            {
                ClassId = classId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Classes", new { id = classId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var announcement = await _context.Announcements
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == announcement.ClassId);

            if (enrollment == null || (enrollment.Role != "Teacher" && announcement.UserId != userId))
            {
                return Forbid();
            }

            var classId = announcement.ClassId;
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Classes", new { id = classId });
        }
    }
}


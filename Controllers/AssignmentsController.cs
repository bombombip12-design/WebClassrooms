using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using FinalASB.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace FinalASB.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int classId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == classId);

            if (enrollment == null || enrollment.Role != "Teacher")
            {
                return Forbid();
            }

            ViewBag.ClassId = classId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Assignment assignment, int? classId)
        {
            try
            {
                // Ensure ClassId is set - try multiple sources
                if (assignment.ClassId == 0)
                {
                    if (classId.HasValue && classId.Value > 0)
                    {
                        assignment.ClassId = classId.Value;
                    }
                    else
                    {
                        var classIdFromViewBag = ViewBag.ClassId as int?;
                        if (classIdFromViewBag.HasValue)
                        {
                            assignment.ClassId = classIdFromViewBag.Value;
                        }
                        else
                        {
                            // Try to get from form data
                            var formClassId = Request.Form["ClassId"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(formClassId) && int.TryParse(formClassId, out int parsedClassId))
                            {
                                assignment.ClassId = parsedClassId;
                            }
                        }
                    }
                }

                // Validate ClassId is set
                if (assignment.ClassId == 0)
                {
                    ModelState.AddModelError("", "Không thể xác định lớp học. Vui lòng thử lại.");
                    ViewBag.ClassId = classId ?? 0;
                    return View(assignment);
                }

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                // Remove validation errors for navigation properties (they will be loaded by EF)
                ModelState.Remove("Class");
                ModelState.Remove("Creator");
                
                // Validate only the fields we care about
                if (string.IsNullOrWhiteSpace(assignment.Title))
                {
                    ModelState.AddModelError("Title", "Tiêu đề là bắt buộc");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ClassId = assignment.ClassId;
                    return View(assignment);
                }
                
                var enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

                if (enrollment == null || enrollment.Role != "Teacher")
                {
                    TempData["ErrorMessage"] = "Bạn không có quyền tạo bài tập cho lớp học này.";
                    return RedirectToAction("Details", "Classes", new { id = assignment.ClassId });
                }

                // Create new assignment object to avoid navigation property validation issues
                var newAssignment = new Assignment
                {
                    ClassId = assignment.ClassId,
                    Title = assignment.Title,
                    Description = assignment.Description,
                    DueDate = assignment.DueDate,
                    CreatedBy = userId,
                    CreatedAt = DateTime.Now
                };

                _context.Assignments.Add(newAssignment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Bài tập đã được tạo thành công!";
                return RedirectToAction("Details", "Classes", new { id = newAssignment.ClassId });
            }
            catch (Exception ex)
            {
                // Log error
                var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AssignmentsController>>();
                logger.LogError(ex, "Error creating assignment: {Message}", ex.Message);
                
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo bài tập. Vui lòng thử lại.";
                ViewBag.ClassId = assignment.ClassId;
                return View(assignment);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .Include(a => a.Creator)
                .Include(a => a.AssignmentFiles)
                .Include(a => a.Submissions)
                    .ThenInclude(s => s.Student)
                .Include(a => a.Submissions)
                    .ThenInclude(s => s.SubmissionFiles)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            // Check if user is enrolled in the class
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

            if (enrollment == null)
            {
                return Forbid();
            }

            ViewBag.UserRole = enrollment.Role;
            ViewBag.UserId = userId;
            ViewBag.UserSubmission = await _context.Submissions
                .Include(s => s.SubmissionFiles)
                .FirstOrDefaultAsync(s => s.AssignmentId == id && s.StudentId == userId);

            return View(assignment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

            if (enrollment == null || enrollment.Role != "Teacher")
            {
                return Forbid();
            }

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return View(assignment);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var existingAssignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == assignment.Id);

            if (existingAssignment == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == existingAssignment.ClassId);

            if (enrollment == null || enrollment.Role != "Teacher")
            {
                return Forbid();
            }

            existingAssignment.Title = assignment.Title;
            existingAssignment.Description = assignment.Description;
            existingAssignment.DueDate = assignment.DueDate;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = assignment.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

            if (enrollment == null || enrollment.Role != "Teacher")
            {
                return Forbid();
            }

            var classId = assignment.ClassId;
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Classes", new { id = classId });
        }
    }
}


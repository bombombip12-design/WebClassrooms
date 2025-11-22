using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalASB.Data;
using FinalASB.Models;
using System.Security.Claims;

namespace FinalASB.Controllers
{
    [Authorize]
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int assignmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

            if (enrollment == null || enrollment.Role != "Student")
            {
                return Forbid();
            }

            ViewBag.AssignmentId = assignmentId;
            ViewBag.Assignment = assignment;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int assignmentId, string driveFileId, string driveFileUrl)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == assignment.ClassId);

            if (enrollment == null || enrollment.Role != "Student")
            {
                return Forbid();
            }

            // Check if submission already exists
            var existingSubmission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == userId);

            Submission submission;

            if (existingSubmission != null)
            {
                submission = existingSubmission;
                submission.SubmittedAt = DateTime.Now;
            }
            else
            {
                submission = new Submission
                {
                    AssignmentId = assignmentId,
                    StudentId = userId,
                    SubmittedAt = DateTime.Now
                };
                _context.Submissions.Add(submission);
            }

            await _context.SaveChangesAsync();

            // Add file if provided
            if (!string.IsNullOrWhiteSpace(driveFileId) && !string.IsNullOrWhiteSpace(driveFileUrl))
            {
                var submissionFile = new SubmissionFile
                {
                    SubmissionId = submission.Id,
                    DriveFileId = driveFileId,
                    DriveFileUrl = driveFileUrl
                };
                _context.SubmissionFiles.Add(submissionFile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Assignments", new { id = assignmentId });
        }

        [HttpPost]
        public async Task<IActionResult> Grade(int submissionId, int? score, string? teacherComment)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var submission = await _context.Submissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Class)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassId == submission.Assignment.ClassId);

            if (enrollment == null || enrollment.Role != "Teacher")
            {
                return Forbid();
            }

            submission.Score = score;
            submission.TeacherComment = teacherComment;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Assignments", new { id = submission.AssignmentId });
        }
    }
}


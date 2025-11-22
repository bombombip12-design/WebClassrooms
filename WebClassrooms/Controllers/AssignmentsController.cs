using Microsoft.AspNetCore.Mvc;
using WebClassrooms.Models;
using Microsoft.EntityFrameworkCore;

namespace WebClassrooms.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AssignmentsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(int? classId)
        {
            var query = _db.Assignments.Include(a => a.Class).AsQueryable();
            if (classId.HasValue) query = query.Where(a => a.ClassId == classId.Value);
            var list = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
            ViewBag.ClassId = classId;
            return View(list);
        }

        public IActionResult Create(int? classId)
        {
            var model = new Assignment();
            if (classId.HasValue) model.ClassId = classId.Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Assignment model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                // TODO set CreatedBy from current user
                model.CreatedBy = 1;
                _db.Assignments.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { classId = model.ClassId });
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var a = await _db.Assignments
                .Include(x => x.Class)
                .Include(x => x.Submissions)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();
            return View(a);
        }
    }
}

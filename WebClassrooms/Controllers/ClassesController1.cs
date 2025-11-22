using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClassrooms.Models;

namespace WebClassrooms.Controllers
{
    public class ClassesController1 : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClassesController1(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Classes
        public async Task<IActionResult> Index()
        {
            var classes = await _db.Classes
                .Include(c => c.Owner)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(classes);
        }

        // GET: /Classes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cls = await _db.Classes
                .Include(c => c.Owner)
                .Include(c => c.Assignments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cls == null) return NotFound();
            return View(cls);
        }

        // GET: /Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;

                // Lưu lớp vào DB
                _db.Classes.Add(model);
                await _db.SaveChangesAsync();

                // Lấy UserID đang đăng nhập
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    ModelState.AddModelError("", "User is not logged in.");
                    return View(model);
                }

                // Thêm vào bảng Enrollment (hoặc ClassMember nếu bạn dùng bảng đó)
                var member = new Enrollment
                {
                    ClassId = model.Id,
                    UserId = userId.Value,
                    Role = "Teacher",
                    JoinedAt = DateTime.Now
                };

                _db.Enrollments.Add(member);
                await _db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }
    }
}

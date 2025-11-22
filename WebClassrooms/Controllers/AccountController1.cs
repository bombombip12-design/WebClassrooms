using Microsoft.AspNetCore.Mvc;
using WebClassrooms.Models;
using Microsoft.EntityFrameworkCore;
using WebClassrooms.Models.ViewModels;

namespace WebClassrooms.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController1(ApplicationDbContext db) => _db = db;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user == null || user.PasswordHash != vm.Password) // demo: lưu bằng text — đổi sang hash
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                return View(vm);
            }
            // TODO: tạo cookie/authenticate user
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (await _db.Users.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(vm);
            }
            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                PasswordHash = vm.Password, // demo: bạn nên hash mật khẩu
                SystemRoleId = 2,
                CreatedAt = DateTime.Now
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Login");
        }
    }
}

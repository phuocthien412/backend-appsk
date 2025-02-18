using _21dthc1DemoAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _21dthc1DemoAPI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        // GET: /Register
        [HttpGet]
        public IActionResult Index()
        {
            // Trỏ đến view trong thư mục Authenticate
            return View("~/Views/Authenticate/Register.cshtml");
        }

        // POST: /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View("~/Views/Authenticate/Register.cshtml", model);
            }

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại!";
                return View("~/Views/Authenticate/Register.cshtml", model);
            }

            IdentityUser user = new()
            {
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return View("~/Views/Authenticate/Register.cshtml", model);
            }

            TempData["SuccessMessage"] = "Đăng ký thành công!";
            return RedirectToAction("Login", "Account");
        }
    }
}
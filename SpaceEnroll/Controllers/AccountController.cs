using Microsoft.AspNetCore.Mvc;
using SpaceEnroll.Data;
using SpaceEnroll.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpaceEnroll.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel()); 
        }

        [HttpGet]
        public IActionResult LoginCoder()
        {
            return View(new LoginCoderViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    _logger.LogInformation("User logged in successfully: {Username}", user.Username);
                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    ModelState.Clear(); // Clear all model errors
                    model.Username = string.Empty;
                    model.Password = string.Empty;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill in both fields.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginCoder(LoginCoderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var coder = await _context.Coders
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (coder != null)
                {
                    _logger.LogInformation("User logged in successfully: {Username}", coder.Username);
                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    ModelState.Clear(); // Clear all model errors
                    model.Username = string.Empty;
                    model.Password = string.Empty;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill in both fields.";
            }

            return View(model);
        }
    }
}

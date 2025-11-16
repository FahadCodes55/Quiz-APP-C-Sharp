using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartQuiz_APP.Models;
using SmartQuiz_APP.Service;
using System.Diagnostics;

namespace SmartQuiz_APP.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly userDbContext _context;

        // Constructor for dependency injection
        public UserController(ILogger<UserController> logger, userDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(login_signup model)
        {
            ViewData["Title"] = "Login";

            
            ModelState.Remove("First_Name");
            ModelState.Remove("Last_Name");
            ModelState.Remove("Id"); 

            if (ModelState.IsValid)
            {
             
                var user = await _context.login_signup
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Roll_no == model.Roll_no);

                if (user != null)
                {
                    // --- CRITICAL FIX: Store User ID in Session ---
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    _logger.LogInformation($"User {model.Email} logged in successfully. User ID: {user.Id} stored in session.");
                    // ------------------------------------------------

                    return RedirectToAction("quiz", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid login attempt. Please check your email and roll number.";
                    _logger.LogWarning($"Failed login attempt for email: {model.Email}");
                    return View(model);
                }
            }
            // If ModelState is not valid or login failed, return to the login view with errors
            return View(model);
        }

        public IActionResult Signup()
        {
            ViewData["Title"] = "Sign Up";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(login_signup model)
        {
            ViewData["Title"] = "Sign Up";

            if (ModelState.IsValid)
            {
                
                if (await _context.login_signup.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    _logger.LogWarning($"Signup failed: Email '{model.Email}' already exists.");
                }
                else if (await _context.login_signup.AnyAsync(u => u.Roll_no == model.Roll_no))
                {
                   
                    TempData["ErrorMessage"] = "This Roll number is already registered. Please use a different roll number.";
                    _logger.LogWarning($"Signup failed: Roll number '{model.Roll_no}' already exists.");
                }
                else
                {
                    // Add new User in database
                    _context.login_signup.Add(model);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"New User {model.Email} signed up successfully!");

                    TempData["SuccessMessage"] = "Account created successfully! Please log in.";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


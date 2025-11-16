/*

     if (ModelState.IsValid)
        {
         // Basic example: Check if a user with the provided email and roll number exists
        // IMPORTANT: This is a VERY basic check. For real authentication:
       // 1. NEVER store plain passwords. Use password hashing (e.g., ASP.NET Core Identity's PasswordHasher or BCrypt.Net).
      // 2. You'll need session management or JWT for persistent login.
// Action to handle login form submission (HTTP POST)
               [HttpPost]
               [ValidateAntiForgeryToken] // Important for security to prevent Cross-Site Request Forgery
               public async Task<IActionResult> Login(login_signup model) // Use your login_signup model or a dedicated LoginViewModel
               {
                   ViewData["Title"] = "Login";

                 var user = await _context.login_signup
                 .FirstOrDefaultAsync(u => u.Email == model.Email && u.Roll_no == model.Roll_no); // Or check hashed password here

                       if (user != null)
                       {
                           // User found - login successful (implement actual sign-in logic here, e.g., SignInManager.SignInAsync)
                           _logger.LogInformation($"User {user.Email} logged in successfully.");
                           return RedirectToAction("Index", "Home"); // Redirect to home page on success
                       }
                       else
                       {
                           ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and roll number.");
                       }
                   }
                   // If ModelState is not valid or login failed, return to the login view with errors
                   return View(model);
               }


               // Action to display the signup form (HTTP GET)
               public IActionResult Signup() // Using PascalCase for action methods is standard
               {
                   ViewData["Title"] = "Sign Up"; // Set page title
                   return View(); // Renders Views/User/Signup.cshtml
               }

               // Action to handle signup form submission (HTTP POST)
               [HttpPost]
               [ValidateAntiForgeryToken] // Important for security
               public async Task<IActionResult> Signup(login_signup model) // Use your login_signup model or a dedicated RegisterViewModel
               {
                   ViewData["Title"] = "Sign Up";

                   if (ModelState.IsValid)
                   {
                       // Check if a user with this email or Roll_no already exists
                       if (await _context.login_signup.AnyAsync(u => u.Email == model.Email))
                       {
                           ModelState.AddModelError("Email", "This email is already registered.");
                       }
                       else if (await _context.login_signup.AnyAsync(u => u.Roll_no == model.Roll_no))
                       {
                           ModelState.AddModelError("Roll_no", "This Roll number is already registered.");
                       }
                       else
                       {
                           // Add the new user to the database
                           _context.login_signup.Add(model); // IMPORTANT: Hash password (model.Password) before saving in a real app!
                           await _context.SaveChangesAsync();

                           _logger.LogInformation($"New user {model.Email} signed up successfully.");
                           return RedirectToAction("Login"); // Redirect to login page after successful signup
                       }
                   }
                   // If ModelState is not valid or user already exists, return to the signup view with errors
                   return View(model);
               }

               // You might want an Error action if this controller handles errors for its views
               // Otherwise, the default Error in HomeController is fine.
               /*
               [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
               public IActionResult Error()
               {
                   return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
               }       




















 */
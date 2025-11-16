using Microsoft.AspNetCore.Mvc;
using SmartQuiz_APP.Models;
using SmartQuiz_APP.Service;
using System.Diagnostics;

namespace SmartQuiz_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly userDbContext _context;

        public HomeController(ILogger<HomeController> logger, userDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        private readonly Dictionary<string, string> _correctAnswers = new Dictionary<string, string>
        {
            {"q1", "c"}, {"q2", "c"}, {"q3", "b"}, {"q4", "c"}, {"q5", "c"},
            {"q6", "c"}, {"q7", "a"}, {"q8", "c"}, {"q9", "c"}, {"q10", "b"}
        };

        private const int TotalQuestions = 10;

        public IActionResult Index()
        {
            ViewData["Title"] = "Home Page";
            return View();
        }

        public IActionResult quiz()
        {
            ViewData["Title"] = "Quiz";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(QuizSubmissionModel model)
        {
            try // <-- TEMPORARY: Wrap the entire method in a try-catch for debugging
            {
                int score = 0;

                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    _logger.LogWarning("Quiz submitted by unauthenticated user. Redirecting to login. Session UserId was null.");
                    return RedirectToAction("Login", "User");
                }

                if (model.Answers != null)
                {
                    foreach (var entry in model.Answers)
                    {
                        string questionName = entry.Key;
                        string selectedAnswer = entry.Value;

                        if (_correctAnswers.TryGetValue(questionName, out string correctAnswer) && selectedAnswer == correctAnswer)
                        {
                            score++;
                        }
                    }
                }

                var quizAttempt = new QuizAttempt
                {
                    UserId = userId.Value,
                    Score = score,
                    TotalQuestions = TotalQuestions,
                    AttemptDate = DateTime.UtcNow
                };

                _context.QuizAttempts.Add(quizAttempt);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Quiz attempt saved for User ID {userId.Value}: Score {score}/{TotalQuestions}");

                ViewData["Score"] = score;
                ViewData["TotalQuestions"] = TotalQuestions;
                ViewData["Title"] = "Quiz Results";

                return RedirectToAction("Answer", new { score = score, total = TotalQuestions });
            }
            catch (Exception ex) // <-- TEMPORARY: Catch all exceptions
            {
                _logger.LogError(ex, "An unhandled exception occurred during quiz submission.");
                // --- TEMPORARY DEBUGGING REDIRECTION ---
                // Redirect to a special debug error page, passing the exception message
                return RedirectToAction("DebugError", new { message = ex.ToString() });
                // ----------------------------------------
            }
        }

        // --- NEW: Temporary DebugError Action for direct exception display ---
        public IActionResult DebugError(string message)
        {
            ViewData["Title"] = "DEBUG ERROR";
            ViewData["ErrorMessage"] = message;
            return View();
        }
        // -------------------------------------------------------------------

        public IActionResult Answer(int score, int total)
        {
            ViewData["Title"] = "Quiz Results";
            ViewData["Score"] = score;
            ViewData["TotalQuestions"] = total;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // This is the default error page; DebugError will bypass this.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
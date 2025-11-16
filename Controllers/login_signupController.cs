using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartQuiz_APP.Models;
using SmartQuiz_APP.Service;

namespace SmartQuiz_APP.Controllers
{
    public class login_signupController : Controller
    {
        private readonly userDbContext _context;

        public login_signupController(userDbContext context)
        {
            _context = context;
        }

        // GET: login_signup
        public async Task<IActionResult> Index()
        {
            return View(await _context.login_signup.ToListAsync());
        }

        // GET: login_signup/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login_signup = await _context.login_signup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (login_signup == null)
            {
                return NotFound();
            }

            return View(login_signup);
        }

        // GET: login_signup/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: login_signup/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,First_Name,Last_Name,Roll_no,Email")] login_signup login_signup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(login_signup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(login_signup);
        }

        // GET: login_signup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login_signup = await _context.login_signup.FindAsync(id);
            if (login_signup == null)
            {
                return NotFound();
            }
            return View(login_signup);
        }

        // POST: login_signup/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,First_Name,Last_Name,Roll_no,Email")] login_signup login_signup)
        {
            if (id != login_signup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(login_signup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!login_signupExists(login_signup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(login_signup);
        }

        // GET: login_signup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login_signup = await _context.login_signup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (login_signup == null)
            {
                return NotFound();
            }

            return View(login_signup);
        }

        // POST: login_signup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var login_signup = await _context.login_signup.FindAsync(id);
            if (login_signup != null)
            {
                _context.login_signup.Remove(login_signup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool login_signupExists(int id)
        {
            return _context.login_signup.Any(e => e.Id == id);
        }
    }
}

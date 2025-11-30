using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET: TrainingSessions
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            var sessions = await _context.TrainingSessions
                .Where(s => s.ApplicationUserId == userId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            return View(sessions);
        }

        // GET: TrainingSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var session = await _context.TrainingSessions
                .Include(s => s.PerformedExercises)
                .ThenInclude(pe => pe.ExerciseType)
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == userId);

            if (session == null) return NotFound();

            return View(session);
        }

        // GET: TrainingSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime")] TrainingSession trainingSession)
        {
            var userId = GetCurrentUserId();
            trainingSession.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(trainingSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingSession);
        }

        // GET: TrainingSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == userId);

            if (session == null) return NotFound();

            return View(session);
        }

        // POST: TrainingSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] TrainingSession trainingSession)
        {
            if (id != trainingSession.Id) return NotFound();
            var userId = GetCurrentUserId();

            var existing = await _context.TrainingSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == userId);

            if (existing == null) return NotFound();

            trainingSession.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _context.TrainingSessions
                        .AnyAsync(s => s.Id == id && s.ApplicationUserId == userId);

                    if (!exists) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainingSession);
        }

        // GET: TrainingSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == userId);

            if (session == null) return NotFound();

            return View(session);
        }

        // POST: TrainingSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == userId);

            if (session != null)
            {
                _context.TrainingSessions.Remove(session);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

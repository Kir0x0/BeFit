using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BeFit.Controllers
{
    [Authorize]
    public class PerformedExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformedExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }
        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET: PerformedExercises
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            var performed = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .Where(p => p.ApplicationUserId == userId)
                .OrderByDescending(p => p.TrainingSession!.StartTime)
                .ToListAsync();

            return View(performed);
        }

        // GET: PerformedExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var performedExercise = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

            if (performedExercise == null) return NotFound();

            return View(performedExercise);
        }

        // GET: PerformedExercises/Create
        public IActionResult Create()
        {
            var userId = GetCurrentUserId();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(ts => ts.ApplicationUserId == userId),
                "Id",
                "StartTime");

            return View();
        }

        // POST: PerformedExercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrainingSessionId,ExerciseTypeId,ApplicationUserId,Weight,Sets,Repetitions")] PerformedExercise performedExercise)
        {
            var userId = GetCurrentUserId();

            var ownSession = await _context.TrainingSessions
                .AnyAsync(ts => ts.Id == performedExercise.TrainingSessionId && ts.ApplicationUserId == userId);

            if (!ownSession)
            {
                ModelState.AddModelError("TrainingSessionId", "Nie możesz dodać ćwiczenia do cudzej sesji.");
            }

            performedExercise.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(performedExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", performedExercise.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(ts => ts.ApplicationUserId == userId),
                "Id",
                "StartTime",
                performedExercise.TrainingSessionId);

            return View(performedExercise);
        }

        // GET: PerformedExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var performedExercise = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

            if (performedExercise == null) return NotFound();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", performedExercise.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(ts => ts.ApplicationUserId == userId),
                "Id",
                "StartTime",
                performedExercise.TrainingSessionId);

            return View(performedExercise);
        }

        // POST: PerformedExercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingSessionId,ExerciseTypeId,ApplicationUserId,Weight,Sets,Repetitions")] PerformedExercise performedExercise)
        {
            if (id != performedExercise.Id) return NotFound();
            var userId = GetCurrentUserId();

            var existing = await _context.PerformedExercises
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

            if (existing == null) return NotFound(); // cudze ćwiczenie

            var ownSession = await _context.TrainingSessions
                .AnyAsync(ts => ts.Id == performedExercise.TrainingSessionId && ts.ApplicationUserId == userId);

            if (!ownSession)
            {
                ModelState.AddModelError("TrainingSessionId", "Nie możesz przenieść ćwiczenia do cudzej sesji.");
            }

            performedExercise.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performedExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _context.PerformedExercises
                        .AnyAsync(p => p.Id == id && p.ApplicationUserId == userId);
                    if (!exists) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", performedExercise.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(ts => ts.ApplicationUserId == userId),
                "Id",
                "StartTime",
                performedExercise.TrainingSessionId);

            return View(performedExercise);
        }

        // GET: PerformedExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();

            var performedExercise = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

            if (performedExercise == null) return NotFound();

            return View(performedExercise);
        }

        // POST: PerformedExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();

            var performedExercise = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

            if (performedExercise != null)
            {
                _context.PerformedExercises.Remove(performedExercise);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PerformedExerciseExists(int id)
        {
            return _context.PerformedExercises.Any(e => e.Id == id);
        }
    }
}

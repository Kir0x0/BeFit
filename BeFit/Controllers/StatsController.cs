using System;
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
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Stats
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.PerformedExercises
                .Include(pe => pe.ExerciseType)
                .Include(pe => pe.TrainingSession)
                .Where(pe => pe.ApplicationUserId == userId &&
                             pe.TrainingSession.StartTime >= fourWeeksAgo)
                .GroupBy(pe => pe.ExerciseType!.Name)
                .Select(g => new ExerciseStatsViewModel
                {
                    ExerciseTypeName = g.Key,
                    TimesPerformed = g.Count(),
                    TotalRepetitions = g.Sum(x => x.Sets * x.Repetitions),
                    AverageWeight = g.Average(x => (double?)x.Weight),
                    MaxWeight = g.Max(x => (double?)x.Weight)
                })
                .OrderBy(s => s.ExerciseTypeName)
                .ToListAsync();

            return View(stats);
        }
    }
}

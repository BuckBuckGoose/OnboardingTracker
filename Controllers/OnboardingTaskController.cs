using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingTracker.Data;
using OnboardingTracker.Models;
using TaskStatus = OnboardingTracker.Models.TaskStatus;

namespace OnboardingTracker.Controllers
{
    [Authorize]
    public class OnboardingTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OnboardingTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OnboardingTask
        public async Task<IActionResult> Index(int? employeeId, string? assignedTo, string? category, TaskStatus? status)
        {
            var currentUser = User.Identity?.Name;
            var query = _context.OnboardingTasks
                .Include(t => t.Employee)
                .Include(t => t.TaskTemplate)
                .AsQueryable();

            // Filter by employee if specified
            if (employeeId.HasValue)
            {
                query = query.Where(t => t.EmployeeId == employeeId.Value);
            }

            // Filter by assigned user
            if (!string.IsNullOrEmpty(assignedTo))
            {
                query = query.Where(t => t.AssignedTo == assignedTo);
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(t => t.Category == category);
            }

            // Filter by status
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            var tasks = await query
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            ViewBag.EmployeeId = employeeId;
            ViewBag.Categories = await _context.OnboardingTasks
                .Select(t => t.Category)
                .Distinct()
                .ToListAsync();

            return View(tasks);
        }

        // GET: OnboardingTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.OnboardingTasks
                .Include(t => t.Employee)
                .Include(t => t.TaskTemplate)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: OnboardingTask/UpdateProgress/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProgress(int id, int progressPercentage)
        {
            var task = await _context.OnboardingTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.ProgressPercentage = Math.Clamp(progressPercentage, 0, 100);
            task.UpdatedAt = DateTime.UtcNow;

            if (progressPercentage > 0 && task.Status == TaskStatus.NotStarted)
            {
                task.Status = TaskStatus.InProgress;
            }

            if (progressPercentage == 100)
            {
                task.Status = TaskStatus.Completed;
                task.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { employeeId = task.EmployeeId });
        }

        // POST: OnboardingTask/MarkComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var task = await _context.OnboardingTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Status = TaskStatus.Completed;
            task.ProgressPercentage = 100;
            task.CompletedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { employeeId = task.EmployeeId });
        }

        // POST: OnboardingTask/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, string assignedTo)
        {
            var task = await _context.OnboardingTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.AssignedTo = assignedTo;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { employeeId = task.EmployeeId });
        }

        // POST: OnboardingTask/UpdateNotes/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNotes(int id, string notes)
        {
            var task = await _context.OnboardingTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Notes = notes;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

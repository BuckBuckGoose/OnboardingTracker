using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingTracker.Data;
using OnboardingTracker.Models;
using TaskStatus = OnboardingTracker.Models.TaskStatus;

namespace OnboardingTracker.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Department,Position,StartDate,IsRemote,NeedsEquipment,NeedsAccessBadge")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedAt = DateTime.UtcNow;
                _context.Add(employee);
                await _context.SaveChangesAsync();

                // Create onboarding tasks based on employee info
                await CreateOnboardingTasks(employee);

                return RedirectToAction("Index", "OnboardingTask", new { employeeId = employee.Id });
            }
            return View(employee);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.OnboardingTasks)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        private async Task CreateOnboardingTasks(Employee employee)
        {
            var taskTemplates = await _context.TaskTemplates
                .Where(t => t.IsActive)
                .ToListAsync();

            foreach (var template in taskTemplates)
            {
                // Check if task should be assigned based on filters
                bool shouldAssign = true;

                if (template.RequiresDepartment && !string.IsNullOrEmpty(template.DepartmentFilter))
                {
                    shouldAssign = employee.Department.Equals(template.DepartmentFilter, StringComparison.OrdinalIgnoreCase);
                }

                if (shouldAssign && template.RequiresRemote && template.RemoteFilter.HasValue)
                {
                    shouldAssign = employee.IsRemote == template.RemoteFilter.Value;
                }

                if (shouldAssign && template.RequiresEquipment && template.EquipmentFilter.HasValue)
                {
                    shouldAssign = employee.NeedsEquipment == template.EquipmentFilter.Value;
                }

                if (shouldAssign && template.RequiresAccessBadge && template.AccessBadgeFilter.HasValue)
                {
                    shouldAssign = employee.NeedsAccessBadge == template.AccessBadgeFilter.Value;
                }

                if (shouldAssign)
                {
                    var task = new OnboardingTask
                    {
                        EmployeeId = employee.Id,
                        TaskTemplateId = template.Id,
                        Title = template.Title,
                        Description = template.Description,
                        Category = template.Category,
                        Status = TaskStatus.NotStarted,
                        ProgressPercentage = 0,
                        DueDate = employee.StartDate.AddDays(template.DaysToComplete),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.OnboardingTasks.Add(task);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnboardingTracker.Models;

namespace OnboardingTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskTemplate> TaskTemplates { get; set; }
        public DbSet<OnboardingTask> OnboardingTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<OnboardingTask>()
                .HasOne(ot => ot.Employee)
                .WithMany(e => e.OnboardingTasks)
                .HasForeignKey(ot => ot.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OnboardingTask>()
                .HasOne(ot => ot.TaskTemplate)
                .WithMany()
                .HasForeignKey(ot => ot.TaskTemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed task templates
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<TaskTemplate>().HasData(
                new TaskTemplate
                {
                    Id = 1,
                    Title = "Complete IT Security Training",
                    Description = "Complete the mandatory IT security and data protection training course",
                    Category = "Training",
                    RequiresDepartment = false,
                    RequiresRemote = false,
                    RequiresEquipment = false,
                    RequiresAccessBadge = false,
                    DaysToComplete = 7,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 2,
                    Title = "Setup Development Environment",
                    Description = "Install and configure all necessary development tools and software",
                    Category = "IT Setup",
                    RequiresDepartment = true,
                    DepartmentFilter = "Engineering",
                    RequiresRemote = false,
                    RequiresEquipment = false,
                    RequiresAccessBadge = false,
                    DaysToComplete = 3,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 3,
                    Title = "Request Equipment",
                    Description = "Submit equipment request for laptop, monitor, keyboard, and mouse",
                    Category = "Equipment",
                    RequiresDepartment = false,
                    RequiresRemote = false,
                    RequiresEquipment = true,
                    EquipmentFilter = true,
                    RequiresAccessBadge = false,
                    DaysToComplete = 14,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 4,
                    Title = "Get Office Access Badge",
                    Description = "Visit security desk to get your office access badge and parking pass",
                    Category = "Facilities",
                    RequiresDepartment = false,
                    RequiresRemote = false,
                    RequiresEquipment = false,
                    RequiresAccessBadge = true,
                    AccessBadgeFilter = true,
                    DaysToComplete = 7,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 5,
                    Title = "Setup Remote Work Tools",
                    Description = "Install VPN, communication tools, and configure remote access",
                    Category = "IT Setup",
                    RequiresDepartment = false,
                    RequiresRemote = true,
                    RemoteFilter = true,
                    RequiresEquipment = false,
                    RequiresAccessBadge = false,
                    DaysToComplete = 3,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 6,
                    Title = "Meet with HR",
                    Description = "Schedule and complete initial HR onboarding meeting to review benefits and policies",
                    Category = "HR",
                    RequiresDepartment = false,
                    RequiresRemote = false,
                    RequiresEquipment = false,
                    RequiresAccessBadge = false,
                    DaysToComplete = 5,
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new TaskTemplate
                {
                    Id = 7,
                    Title = "Department Orientation",
                    Description = "Attend department orientation and meet team members",
                    Category = "Training",
                    RequiresDepartment = false,
                    RequiresRemote = false,
                    RequiresEquipment = false,
                    RequiresAccessBadge = false,
                    DaysToComplete = 7,
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );
        }
    }
}

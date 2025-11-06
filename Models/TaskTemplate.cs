using System.ComponentModel.DataAnnotations;

namespace OnboardingTracker.Models
{
    public class TaskTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        // Conditions for when this task should be assigned
        public bool RequiresDepartment { get; set; }
        public string? DepartmentFilter { get; set; }

        public bool RequiresRemote { get; set; }
        public bool? RemoteFilter { get; set; }

        public bool RequiresEquipment { get; set; }
        public bool? EquipmentFilter { get; set; }

        public bool RequiresAccessBadge { get; set; }
        public bool? AccessBadgeFilter { get; set; }

        public int DaysToComplete { get; set; } = 7;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

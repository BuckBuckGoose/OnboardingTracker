using System.ComponentModel.DataAnnotations;

namespace OnboardingTracker.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public bool IsRemote { get; set; }

        public bool NeedsEquipment { get; set; }

        public bool NeedsAccessBadge { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<OnboardingTask> OnboardingTasks { get; set; } = new List<OnboardingTask>();
    }
}

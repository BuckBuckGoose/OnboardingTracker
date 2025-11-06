using System.ComponentModel.DataAnnotations;

namespace OnboardingTracker.Models
{
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class OnboardingTask
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;

        public int TaskTemplateId { get; set; }
        public virtual TaskTemplate TaskTemplate { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        public int ProgressPercentage { get; set; } = 0;

        [StringLength(100)]
        public string? AssignedTo { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }
    }
}

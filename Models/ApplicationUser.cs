using Microsoft.AspNetCore.Identity;

namespace OnboardingTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Department { get; set; }
        public bool IsManager { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace ims.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Extra profile info beyond default Identity fields
        public string? FullName { get; set; }

        // Useful for enabling/disabling accounts
        public bool IsActive { get; set; } = true;
    }
}

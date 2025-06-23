using Microsoft.AspNetCore.Identity;

namespace Realestate.Entities
{
    public class User : IdentityUser
    {
        public string? FNameEn { get; set; }
        public string? FNameAr { get; set; }

        public string? LNameEn { get; set; }
        public string? LNameAr { get; set; }

        public string Role { get; set; } = "Admin";
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? Image { get; set; }
    }
}

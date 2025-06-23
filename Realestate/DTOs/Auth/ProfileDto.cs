namespace Realestate.DTOs.Auth
{
    public class ProfileDto
    {
        public string FNameAr { get; set; } = default!;
        public string FNameEn { get; set; } = default!;
        public string LNameAr { get; set; } = default!;
        public string LNameEn { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? ImageUrl { get; set; }
    }
}

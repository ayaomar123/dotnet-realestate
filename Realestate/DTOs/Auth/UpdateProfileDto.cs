namespace Realestate.DTOs.Auth
{
    public class UpdateProfileDto
    {
        public string FNameAr { get; set; } = string.Empty;
        public string FNameEn { get; set; } = string.Empty;
        public string LNameAr { get; set; } = string.Empty;
        public string LNameEn { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}

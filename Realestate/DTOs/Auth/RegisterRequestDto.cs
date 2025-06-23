using System.ComponentModel.DataAnnotations;

namespace Realestate.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(30)]
        public string FNameEn { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string FNameAr { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string LNameEn { get; set; } = string.Empty;


        [Required]
        [MaxLength(30)]
        public string LNameAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}

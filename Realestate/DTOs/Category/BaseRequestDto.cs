using System.ComponentModel.DataAnnotations;

namespace Realestate.DTOs.Category
{
    public class BaseRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string NameEn { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string NameAr { get; set; }
        
        public IFormFile? Image { get; set; }= null;

    }
}

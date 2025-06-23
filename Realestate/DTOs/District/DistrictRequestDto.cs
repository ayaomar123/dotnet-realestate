using System.ComponentModel.DataAnnotations;

namespace Realestate.DTOs.District
{
    public class DistrictRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string NameEn { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string NameAr { get; set; }
        [Required]
        public int CityId { get; set; }
        public IFormFile Image { get; set; }

    }
}

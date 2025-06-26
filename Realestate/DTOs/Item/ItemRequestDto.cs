using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Realestate.DTOs.Item
{
    public class ItemRequestDto
    {
        [Required]
        [StringLength(255)]
        public string NameEn { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string NameAr { get; set; } = null!;

        public IFormFile? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public int MyTypeId { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int AdvertiseNo { get; set; }

        [Required]
        public int AdNo { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public double Soum { get; set; }

        public double Limit { get; set; }

        public double StreetWidth { get; set; }

        public double Space { get; set; }

        public double PricePerMeter { get; set; }

        [MaxLength(2000)]
        public string? DescriptionEn { get; set; }

        [MaxLength(2000)]
        public string? DescriptionAr { get; set; }

        public bool HasUnits { get; set; } = false;

        public double RengeFrom { get; set; }

        public double RangeTo { get; set; }

        public bool HasPassword { get; set; }

        [StringLength(255)]
        public string? HashPassword { get; set; }

        public List<IFormFile> Images { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;

namespace Realestate.DTOs.Item
{
    public class ItemRequestDto : IValidatableObject
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

        //[Required]
        public double Latitude { get; set; }

        //[Required]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasUnits)
            {
                //if (RengeFrom <= 0)
                  //  yield return new ValidationResult("RengeFrom is required.", new[] { nameof(RengeFrom) });

                if (RangeTo <= RengeFrom)
                    yield return new ValidationResult("RangeTo must be greater than RengeFrom.", new[] { nameof(RangeTo) });
            }

            if (HasPassword && string.IsNullOrWhiteSpace(HashPassword))
            {
                yield return new ValidationResult("HashPassword is required when HasPassword is true.", new[] { nameof(HashPassword) });
            }
        }
    }
}

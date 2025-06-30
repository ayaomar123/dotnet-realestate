using System.ComponentModel.DataAnnotations;

namespace Realestate.DTOs.Item
{
    public class ItemUpdateRequestDto : IValidatableObject
    {
        [StringLength(255)]
        public string? NameEn { get; set; }

        [StringLength(255)]
        public string? NameAr { get; set; }

        public IFormFile? Image { get; set; }

        public int? CategoryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? MyTypeId { get; set; }
        public int? PropertyTypeId { get; set; }
        public int? StatusId { get; set; }
        public int? AdvertiseNo { get; set; }
        public int? AdNo { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public double? Soum { get; set; }
        public double? Limit { get; set; }
        public double? StreetWidth { get; set; }
        public double? Space { get; set; }
        public double? PricePerMeter { get; set; }

        [MaxLength(2000)]
        public string? DescriptionEn { get; set; }

        [MaxLength(2000)]
        public string? DescriptionAr { get; set; }

        public bool? HasUnits { get; set; }
        public double? RengeFrom { get; set; }
        public double? RangeTo { get; set; }

        public bool? HasPassword { get; set; }

        [StringLength(255)]
        public string? HashPassword { get; set; }

        public List<IFormFile>? Images { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasUnits == true)
            {
                if (RengeFrom == null || RengeFrom <= 0)
                    yield return new ValidationResult("RengeFrom is required when HasUnits is true.", new[] { nameof(RengeFrom) });

                if (RangeTo == null || RangeTo <= RengeFrom)
                    yield return new ValidationResult("RangeTo must be greater than RengeFrom.", new[] { nameof(RangeTo) });
            }

            if (HasPassword == true && string.IsNullOrWhiteSpace(HashPassword))
            {
                yield return new ValidationResult("HashPassword is required when HasPassword is true.", new[] { nameof(HashPassword) });
            }
        }
    }

}

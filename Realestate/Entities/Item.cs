using System.ComponentModel.DataAnnotations.Schema;

namespace Realestate.Entities
{
    public class Item : BaseEntity
    {
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public int DistrictId { get; set; }
        public District? District { get; set; }
        public int MyTypeId { get; set; }
        public MyType? MyType { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }

        public int StatusId { get; set; }
        public Status? Status { get; set; }

        public int AdvertiseNo { get; set; }

        public int AdNo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Soum { get; set; }
        public double Limit { get; set; }
        public double StreetWidth { get; set; }
        public double Space { get; set; }
        public double PricePerMeter { get; set; }

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public bool HasUnits { get; set; } = false;

        public double RengeFrom { get; set; }
        public double RangeTo { get; set; }

        public bool HasPassword { get; set; }

        public string? HashPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Image> Images { get; set; } = new();

    }
}

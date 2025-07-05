namespace Realestate.DTOs.Item
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }

        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int MyTypeId { get; set; }
        public string? MyTypeName { get; set; }
        public int PropertyTypeId { get; set; }
        public string? PropertyTypeName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? StatusColor { get; set; }
        public string? StatusBackgroundColor { get; set; }

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
        public bool IsActive { get; set; }

        public string? HashPassword { get; set; }

        public List<string> Images { get; set; } = new(); // يجب تكون List<string> أو DTO مخصص
    }
}

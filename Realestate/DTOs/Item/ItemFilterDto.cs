using Realestate.DTOs.Paginations;

namespace Realestate.DTOs.Item
{
    public class ItemFilterDto : PaginationDto
    {
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public int? DistrictId { get; set; }
        public int? PropertyTypeId { get; set; }
        public int? MyTypeId { get; set; }
        public int? StatusId { get; set; }
        public string? Keyword { get; set; }
        public int? AdNo { get; set; }
        public int? AdvertiseNo { get; set; }
        public bool? IsActive { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Realestate.Entities
{
    public class District : BaseEntity
    {
        public int CityId { get; set; }
        public City? City { get; set; }
        
    }
}

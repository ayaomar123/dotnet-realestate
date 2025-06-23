using System.ComponentModel.DataAnnotations;

namespace Realestate.Entities
{
    public class City : BaseEntity { 

        public ICollection<District>? Districts { get; set; }
    }
}

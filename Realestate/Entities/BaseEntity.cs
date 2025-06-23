using System.ComponentModel.DataAnnotations;

namespace Realestate.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string? NameEn { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string? NameAr { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}

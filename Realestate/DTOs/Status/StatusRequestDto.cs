using System.ComponentModel.DataAnnotations;
using Realestate.DTOs.Category;

namespace Realestate.DTOs.Status
{
    public class StatusRequestDto : BaseRequestDto
    {
        public string? Color { get; set; }

        public string? BackgroundColor { get; set; }

        public int Sort { get; set; } = 1;

    }
}

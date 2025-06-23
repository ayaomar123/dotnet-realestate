namespace Realestate.Entities
{
    public class Status : BaseEntity
    {
        public string? Color { get; set; }

        public string? BackgroundColor { get; set; }

        public int Sort { get; set; } = 1;
    }
}

namespace Realestate.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}

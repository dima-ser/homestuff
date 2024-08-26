namespace HomeStuff.Models
{
    public class ItemSet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Item> Items { get; } = new List<Item>();
    }
}

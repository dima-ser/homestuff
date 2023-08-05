using System.ComponentModel.DataAnnotations;
namespace HomeStuff.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public ICollection<Item> Items { get; } = new List<Item>();
        public override string ToString()
        {
            return Name;
        }
    }
}

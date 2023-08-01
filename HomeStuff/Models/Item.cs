using System.ComponentModel.DataAnnotations;

namespace HomeStuff.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}

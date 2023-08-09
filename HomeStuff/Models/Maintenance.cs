using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStuff.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public Item Item { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateOnly Date { get; set; }
        public bool Completed { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStuff.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        // Entity Framework is stupid
        public Item? Item { get; set; }
        public string Description { get; set; } = string.Empty;
        //public string? Notes { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //This will be local date since it won't be accurate anyway to convert DateOnly to/from Utc
        public DateOnly Date { get; set; }
        [DisplayName("Status")]
        public bool Completed { get; set; }
    }
}

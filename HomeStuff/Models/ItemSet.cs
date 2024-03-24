using System.ComponentModel;

namespace HomeStuff.Models
{

    public class ItemSet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [DisplayName("Location")]
        public required int LocationId { get; set; }
        public Location? Location { get; set; } // need to make this nullable to get binding to work.
        public override string ToString()
        {
            return Name;
        }
    }
}

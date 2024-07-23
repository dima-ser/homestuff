using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HomeStuff.Models
{
    public class Location
    {
        public int Id { get; set; }
        [DisplayName("Parent Location")]
        public int? ParentId { get; set; }
        public string FullName { get; set; } = "";
        [Required]
        public required string Name { get; set; }
        public ICollection<Item> Items { get; } = new List<Item>();
        //public override string ToString()
        //{
        //    return Name;
        //}
        public static string SUBLOCATION_DIVIDER = " / ";
    }
}

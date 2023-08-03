using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeStuff.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required, DisplayName("Location")]
        public int LocationId {  get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        [DisplayName("Model Number")]
        public string? ModelNumber { get; set; }
        [DisplayName("Serial Number")]
        public string? SerialNumber { get; set; }

        [DisplayName("Purchase Price")]
        public double? PurchasePrice { get; set; }
        public string? Vendor { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Purchase Date")]
        public DateOnly? PurchaseDate { get; set; }
        public string? SKU { get; set; }

    }
}

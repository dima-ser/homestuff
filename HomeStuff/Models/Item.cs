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

        [DisplayName("Purchase Price")]
        public double? PurchasePrice { get; set; }


    }
}

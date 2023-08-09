using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HomeStuff.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required, DisplayName("Location")]
        public int LocationId {  get; set; }
        // need to make this nullable to get binding to work. It doesn't affect db anyway as only LocationID goes into db
        public Location? Location { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        [DisplayName("Model Number")]
        public string? ModelNumber { get; set; }
        [DisplayName("Serial Number")]
        public string? SerialNumber { get; set; }

        [DisplayName("Purchase Price"), DataType(DataType.Currency)]
        public double? PurchasePrice { get; set; }
        public string? Vendor { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Purchase Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? PurchaseDate { get; set; }
        public string? SKU { get; set; }
        public DateTime LastModifiedUtc { get; set; }

        /// <summary>
        /// Returns a physical path of a directory on the server where attachments for the given itemId are located
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static string GetAttachmentDirPhysicalPath(IWebHostEnvironment environment, IConfiguration configuration, string itemId)
        {
            string attachmentDir = Path.Combine(environment.ContentRootPath, configuration.GetValue<string>(Utilities.ConfigUserDataDirectory));
            attachmentDir = Path.Combine(attachmentDir, Utilities.AttachmentDirName);
            return Path.Combine(attachmentDir, itemId);
        }
    }

}

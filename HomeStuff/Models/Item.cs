using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Mail;
using System.Security.Policy;
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
        public string? MaintenanceNotes { get; set; }
        public ICollection<Maintenance> Maintenances { get;} = new List<Maintenance>();

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

        public static bool HasAttachments(IWebHostEnvironment environment, IConfiguration configuration, int itemId)
        {
            string attachmentDir = GetAttachmentDirPhysicalPath(environment, configuration, itemId.ToString());
            return Directory.Exists(attachmentDir) && Directory.EnumerateFiles(attachmentDir).Any();
        }

        public static List<ItemAttachment> GetAttachments(IWebHostEnvironment environment, IConfiguration configuration, PageModel pageModel, int itemId)
        {
            List<ItemAttachment> attachments = new List<ItemAttachment>();
            string attachmentDir = GetAttachmentDirPhysicalPath(environment, configuration, itemId.ToString());
            if (Directory.Exists(attachmentDir))
            {
                var files = Directory.EnumerateFiles(attachmentDir);
                foreach (var file in files)
                {
                    FileInfo fi = new(file);
                    attachments.Add(new ItemAttachment(
                        file,
                        pageModel.Url.Content("~/itemattachment?itemid=" + itemId + "&name=" + HttpUtility.UrlEncode(fi.Name)),
                        pageModel.Url.Content("~/itemattachmentdelete?itemid=" + itemId + "&name=" + HttpUtility.UrlEncode(fi.Name)),
                    fi.Name,
                        (Math.Ceiling((float)fi.Length / 1024.0)).ToString() + " KB"));
                }
                attachments.Sort();
            }
            return attachments;
        }
    }

}

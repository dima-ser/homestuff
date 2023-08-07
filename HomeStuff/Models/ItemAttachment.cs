using Microsoft.AspNetCore.Hosting;
using System.Web;

namespace HomeStuff.Models
{
    public class ItemAttachment
    {
        public int Id { get; set; }
        public string PhysicalPath { get; set; }
        public string Url { get; set; }
        public string DeleteUrl { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }

        public ItemAttachment(string physicalPath, string url, string deleteUrl, string name, string size)
        {
            this.PhysicalPath = physicalPath;
            this.Url = url;
            this.DeleteUrl = deleteUrl;
            this.Name = name;
            this.Size = size;

        }   
        public static string GetPhysicalPath(IWebHostEnvironment environment, IConfiguration configuration, string itemId, string attachmentName)
        {
            string attachmentDir = Path.Combine(environment.ContentRootPath, configuration.GetValue<string>("AttachmentDir"));
            attachmentDir = Path.Combine(attachmentDir, itemId);
            string fileName = HttpUtility.UrlDecode(attachmentName);
            return Path.Combine(attachmentDir, fileName);
        }
    }
}

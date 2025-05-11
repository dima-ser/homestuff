using HomeStuff.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Build.Framework;
using NuGet.Protocol;
using System.Configuration;
using System.Net.Mail;
using System.Web;

namespace HomeStuff.Pages
{
    [ResponseCache(Duration = 604800)]
    public class ItemAttachmentModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty(SupportsGet =true)]
        public string? ItemID { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        public ItemAttachmentModel(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }


        public ActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(ItemID) && !string.IsNullOrEmpty(Name))
            {
                //string fileName = HttpUtility.UrlDecode(Name);
                // apparently, Name here is automatically URL decoded by the framework?
                string fileName = Name;
                string attachmentPath = ItemAttachment.GetPhysicalPath(webHostEnvironment, configuration, ItemID, fileName);
                if (System.IO.File.Exists(attachmentPath))
                {
                    var fileProvider = new FileExtensionContentTypeProvider();
                    // Figures out what the content type should be based on the file name.  
                    if (!fileProvider.TryGetContentType(fileName, out string? contentType))
                    {
                        Console.WriteLine("Unable to determine attachment's MIME content type, using default \"application/octet-stream\"");
                    }
                    if (string.IsNullOrEmpty(contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    return File(System.IO.File.OpenRead(attachmentPath), contentType, fileName);
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }
    }
}

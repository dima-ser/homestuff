using HomeStuff.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Web;

namespace HomeStuff.Pages
{
    public class ItemAttachmentDeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly Data.SqliteContext _context;
        [BindProperty(SupportsGet =true)]
        public int? ItemId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }
        public ItemAttachmentDeleteModel(Data.SqliteContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment= webHostEnvironment;
            _configuration= configuration;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (ItemId == null || string.IsNullOrEmpty(Name))
            {
                return NotFound();
            }
            //string fileName = HttpUtility.UrlDecode(Name);
            // apparently, Name here is automatically URL decoded by the framework?
            string attachmentPath = ItemAttachment.GetPhysicalPath(_webHostEnvironment, _configuration, ItemId.ToString()!, Name);

            if (System.IO.File.Exists(attachmentPath))
            {
                System.IO.File.Delete(attachmentPath);
                var item = _context.Item.FirstOrDefault(i => i.Id == ItemId);
                if (item != null)
                {
                    item.LastModifiedUtc = DateTime.UtcNow;
                    _context.SaveChangesAsync();
                }
                // if this was the last attachment, also delete the directory
                string attachmentDir = Item.GetAttachmentDirPhysicalPath(_webHostEnvironment, _configuration, ItemId.ToString()!);
                if (Directory.EnumerateFiles(attachmentDir).Count() == 0)
                {
                    Directory.Delete(attachmentDir);
                }
                return Redirect("./Item?id=" + ItemId);
            }
            else
                return NotFound();
        }
    }
}

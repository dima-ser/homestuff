using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;
using System.Web;

namespace HomeStuff.Pages
{
    public class ItemModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeStuff.Models.Item Item { get; set; } = default!;
        public string LocationName { get; set; } = string.Empty;
        public List<ItemAttachment> Attachments { get; set; } = new List<ItemAttachment>();

        public ItemModel(HomeStuff.Data.SqliteContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                Item = item;
                LocationName = _context.Location.Where(l => l.Id == Item.LocationId).First().Name;

                string attachmentDir = Path.Combine(webHostEnvironment.ContentRootPath, configuration.GetValue<string>("AttachmentDir"));
                attachmentDir = Path.Combine(attachmentDir, Item.Id.ToString());
                if (Directory.Exists(attachmentDir))
                {
                    var files = Directory.EnumerateFiles(attachmentDir);
                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        Attachments.Add(new ItemAttachment(
                            file, 
                            Url.Content("~/itemattachment?itemid=" + Item.Id + "&name=" + HttpUtility.UrlEncode(fi.Name)),
                            Url.Content("~/itemattachmentdelete?itemid=" + Item.Id + "&name=" + HttpUtility.UrlEncode(fi.Name)),
                            fi.Name,
                            (Math.Ceiling((float)fi.Length / 1024.0)).ToString() + " KB"));
                    }
                }
            }
            ViewData["Title"] = Item.Name;
            return Page();
        }
    }
}

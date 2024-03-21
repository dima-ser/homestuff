using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;
using System.Web;
using System.ComponentModel;

namespace HomeStuff.Pages
{
    public class ItemModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeStuff.Models.Item Item { get; set; } = default!;
        public string LocationFullName { get; set; } = string.Empty;
        public List<ItemAttachment> Attachments { get; set; } = new List<ItemAttachment>();
        [DisplayName("Attachment")]
        public IFormFile? AttachmentFile { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ErrorMessage { get; set; } = string.Empty;

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
                LocationFullName = _context.Location.Where(l => l.Id == Item.LocationId).First().FullName;
                Attachments = Item.GetAttachments(webHostEnvironment, configuration, this, item.Id);
               
            }
            ViewData["Title"] = Item.Name;
            return Page();
        }

        public async Task<ActionResult> OnPost(int? id)
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
            if (AttachmentFile != null)
            {
                if (AttachmentFile.Length > 0)
                {
                    try
                    {
                        string directoryPath = Item.GetAttachmentDirPhysicalPath(webHostEnvironment, configuration, id.ToString()!);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filePath = Path.Combine(directoryPath, AttachmentFile.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            return Redirect("./item?errormessage=Attachment+with+that+name+already+exists&id=" + id);
                        }
                        else
                        {
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                AttachmentFile.CopyTo(stream);
                            }
                            item.LastModifiedUtc = DateTime.UtcNow;
                            await _context.SaveChangesAsync();
                        }

                    }
                    catch (Exception) { throw; }
                }
            }
            else
                Console.WriteLine("Attachmentfile is null");
            return Redirect("./Item?id=" + id);
        }
    }
}

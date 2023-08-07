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
        [BindProperty(SupportsGet =true)]
        public string? ItemId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }
        public ItemAttachmentDeleteModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment= webHostEnvironment;
            _configuration= configuration;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ItemId) || string.IsNullOrEmpty(Name))
            {
                return NotFound();
            }
            string fileName = HttpUtility.UrlDecode(Name);
            string attachmentPath = ItemAttachment.GetPhysicalPath(_webHostEnvironment, _configuration, ItemId, fileName);
            Console.WriteLine("physical path: " + attachmentPath);
            if (System.IO.File.Exists(attachmentPath))
            {
                System.IO.File.Delete(attachmentPath);
                return Redirect("./Item?id=" + ItemId);
            }
            else
                return NotFound();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeStuff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HomeStuff.Data.SqliteContext _context;
        public IList<HomeStuff.Models.Item> Items { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, HomeStuff.Data.SqliteContext context)
        {
            _logger = logger;
            _context = context;
        }


        public void OnGet()
        {
            if (_context.Item != null)
            {
                Items =  _context.Item.OrderByDescending(i => i.LastModifiedUtc).ToList();
            }
        }
    }
}
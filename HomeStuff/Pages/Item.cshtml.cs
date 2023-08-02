using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class ItemModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public HomeStuff.Models.Item Item { get; set; } = default!;
        public string LocationName { get; set; } = string.Empty;

        public ItemModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
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
            }
            ViewData["Title"] = Item.Name;
            return Page();
        }
    }
}

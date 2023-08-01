using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class ItemModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public ItemModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public HomeStuff.Models.Item Item { get; set; } = default!;

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
            }
            return Page();
        }
    }
}

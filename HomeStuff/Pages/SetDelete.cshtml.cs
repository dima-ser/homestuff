using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Data;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class SetDeleteModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public ICollection<HomeStuff.Models.Item> Items { get; set; } = new List<HomeStuff.Models.Item>();

        public SetDeleteModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;

        }

        [BindProperty]
      public ItemSet ItemSet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ItemSet == null)
            {
                return NotFound();
            }

            var set = await _context.ItemSet.FirstOrDefaultAsync(m => m.Id == id);

            if (set == null)
            {
                return NotFound();
            }
            else 
            {
                ItemSet = set;
                Items = _context.Item.Where(i => i.ItemSetId == id).ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ItemSet == null)
            {
                return NotFound();
            }
            var set = await _context.ItemSet.FindAsync(id);

            if (set != null)
            {
                ItemSet = set;
                _context.ItemSet.Remove(ItemSet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Sets");
        }
    }
}

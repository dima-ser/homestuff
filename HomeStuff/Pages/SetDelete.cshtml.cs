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

            var itemset = await _context.ItemSet.FirstOrDefaultAsync(m => m.Id == id);

            if (itemset == null)
            {
                return NotFound();
            }
            else if (_context.Item.Where(m => m.ItemSetId == itemset.Id).Any()) // prevent deleting set if it has any items
            {
                return BadRequest();
            }
            else 
            {
                ItemSet = itemset;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ItemSet == null)
            {
                return NotFound();
            }
            var itemset = await _context.ItemSet.FindAsync(id);

            if (itemset != null)
            {
                ItemSet = itemset;
                _context.ItemSet.Remove(ItemSet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Sets");
        }
    }
}

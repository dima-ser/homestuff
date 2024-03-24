using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Data;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class SetEditModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public SetEditModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ItemSet ItemSet { get; set; } = default!;
        public SelectList Locations { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ItemSet == null)
            {
                return NotFound();
            }

            var itemSet =  await _context.ItemSet.FirstOrDefaultAsync(m => m.Id == id);
            if (itemSet == null)
            {
                return NotFound();
            }
            ItemSet = itemSet;
            Locations = new SelectList(_context.Location.OrderBy(i => i.FullName), nameof(Location.Id), nameof(Location.FullName));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ItemSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemSetExists(ItemSet.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Sets");
        }

        private bool ItemSetExists(int id)
        {
          return (_context.ItemSet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

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
    public class ItemEditModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public SelectList Locations { get; set; }
        public SelectList Sets { get; set; }
        [BindProperty]
        public HomeStuff.Models.Item Item { get; set; } = default!;

        public ItemEditModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
            Locations = new SelectList(context.Location.OrderBy(i => i.FullName), nameof(Location.Id), nameof(Location.FullName));
            Sets = new SelectList(context.ItemSet.OrderBy(i => i.Name), nameof(ItemSet.Id), nameof(ItemSet.Name));
            ItemStatuses = new SelectList(Enum.GetValues(typeof(Item.ItemStatus)));
            
        }

       public SelectList ItemStatuses { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item =  await _context.Item.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Item = item;
            ViewData["Title"] = Item.Name;
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

            Item.LastModifiedUtc = DateTime.UtcNow;
            _context.Attach(Item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Item", new { id = Item.Id });
        }

        private bool ItemExists(int id)
        {
          return (_context.Item?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

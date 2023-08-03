using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeStuff.Data;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class ItemNewModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public SelectList Locations { get; set; }
        [BindProperty]
        public HomeStuff.Models.Item Item { get; set; } = default!;

        public ItemNewModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
            Locations = new SelectList(context.Location, nameof(Location.Id), nameof(Location.Name));
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Item == null || Item == null)
            {
                return Page();
            }
            Item.LastModifiedUtc = DateTime.UtcNow;
            _context.Item.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

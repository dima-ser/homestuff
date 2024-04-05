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
    public class LocationNewModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public string? RedirectUrl { get; set; } = null;
        public SelectList RootLocations { get; set; }

        public LocationNewModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
            RootLocations = new SelectList(_context.Location.Where(i => i.ParentId == null).OrderBy(i => i.Name), nameof(Location.Id), nameof(Location.Name));

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Location Location { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Location == null || Location == null)
            {
                return Page();
            }
            if (Location.ParentId == null)
                Location.FullName = Location.Name;
            else
                Location.FullName = _context.Location.Where(l => l.Id == Location.ParentId).First().Name + Location.SUBLOCATION_DIVIDER + Location.Name;
            _context.Location.Add(Location);
            await _context.SaveChangesAsync();
            if (string.IsNullOrEmpty(RedirectUrl))
                return RedirectToPage("./Locations");
            else
                return Redirect(RedirectUrl);
        }
    }
}

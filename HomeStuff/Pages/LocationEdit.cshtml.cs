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
using HomeStuff.Migrations;

namespace HomeStuff.Pages
{
    public class LocationEditModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public LocationEditModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Location Location { get; set; } = default!;
        public SelectList RootLocations { get; set; }= default!;
        public bool ParentReadOnly = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location =  await _context.Location.FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }
            Location = location;
            RootLocations = new SelectList(_context.Location.Where(l=>l.ParentId==null).OrderBy(i => i.Name), nameof(Location.Id), nameof(Location.Name));
            if (_context.Location.Where(l=>l.ParentId==Location.Id).Any())
            {
                ParentReadOnly = true;
            }
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
            if (Location.ParentId == null)
                Location.FullName = Location.Name;
            else
                Location.FullName = _context.Location.Where(l => l.Id == Location.ParentId).First().Name + " > " + Location.Name;

            _context.Attach(Location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(Location.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Locations");
        }

        private bool LocationExists(int id)
        {
          return (_context.Location?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

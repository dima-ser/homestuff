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
    public class MaintEditModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public MaintEditModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Maintenance Maintenance { get; set; } = default!;
        [BindProperty]
        public string? ItemName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Maintenance == null)
            {
                return NotFound();
            }

            var maintenance =  await _context.Maintenance.FirstOrDefaultAsync(m => m.Id == id);
            if (maintenance == null)
            {
                return NotFound();
            }
            Maintenance = maintenance;
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            ItemName = _context.Item.FirstOrDefault(i => i.Id == maintenance!.ItemId).Name;
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

            _context.Attach(Maintenance).State = EntityState.Modified;
            Item Item = await _context.Item.FindAsync(Maintenance.ItemId);
            Item.LastModifiedUtc = DateTime.UtcNow;
            _context.Attach(Item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(Maintenance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Maintenance", new { itemid = Maintenance.ItemId.ToString() });
        }

        private bool MaintenanceExists(int id)
        {
          return (_context.Maintenance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

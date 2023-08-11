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
    public class MaintDeleteModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public MaintDeleteModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Maintenance Maintenance { get; set; } = default!;
        [BindProperty]
        public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Maintenance == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenance.FirstOrDefaultAsync(m => m.Id == id);
            //var item = await _context.Item.FirstOrDefaultAsync(m => m.Id == ItemId);

            if (maintenance == null)
            {
                return NotFound();
            }
            else 
            {
                Maintenance = maintenance;
                Item = await _context.Item.FirstOrDefaultAsync(m => m.Id == maintenance.ItemId);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Maintenance == null)
            {
                return NotFound();
            }
            //if (ItemId == null)
            //{
            //    Console.WriteLine("ItemId is null on POST");
            //    return NotFound();
            //}
            var maintenance = await _context.Maintenance.FindAsync(id);
            int ItemId;
            if (maintenance != null)
            {
                Maintenance = maintenance;
                ItemId = maintenance.ItemId;
                Item Item = await _context.Item.FindAsync(maintenance.ItemId);
                _context.Maintenance.Remove(Maintenance);
                Item.LastModifiedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Maintenance", new { itemid = ItemId.ToString() });
            }
            else
                return NotFound();
        }
    }
}

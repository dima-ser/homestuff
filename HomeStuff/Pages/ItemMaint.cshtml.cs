using HomeStuff.Data;
using HomeStuff.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HomeStuff.Pages
{
    public class ItemMaintModel : PageModel
    {
        private readonly SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public int ItemId { get; set; }
        public List<Models.Maintenance> Maintenances { get; set; } = new List<Models.Maintenance>();
        public Models.Item Item = default!;

        public ItemMaintModel(SqliteContext context) 
        { 
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? ItemId)
        {
            if (ItemId == null)
            {
                return NotFound();
            }
            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (Item == null)
            {
                return NotFound();
            }
            Maintenances = _context.Maintenance.Where(i => i.Item.Id == ItemId).ToList();
            ViewData["Title"] = Item.Name;
            return Page();
        }
    }
}

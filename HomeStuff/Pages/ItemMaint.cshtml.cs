using HomeStuff.Data;
using HomeStuff.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HomeStuff.Pages
{
    public class ItemMaintModel : PageModel
    {
        private readonly SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public int ItemId { get; set; }
        public List<Models.Maintenance> Maintenances { get; set; } = new List<Models.Maintenance>();
        public Models.Item? Item;

        [BindProperty]
        public Models.Maintenance NewMaintenance { get; set; } = new Models.Maintenance();

        public ItemMaintModel(SqliteContext context) 
        { 
            _context = context;
            NewMaintenance.Date = DateOnly.FromDateTime(DateTime.Now);
        }

        public async Task<IActionResult> OnGetAsync(int ItemId)
        {
            //if (ItemId == null)
            //{
            //    return NotFound();
            //}
            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (Item == null)
            {
                return NotFound();
            }
            Maintenances = _context.Maintenance.Where(i => i.Item!.Id == ItemId).ToList();
            NewMaintenance.ItemId = Item.Id;
            ViewData["Title"] = Item.Name;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (ItemId == null)
            //{
            //    return NotFound();
            //}
            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (NewMaintenance == null || Item == null)
            {
                return Page();
            }
            //NewMaintenance.Item = Item;
            if (!ModelState.IsValid)
            {
                Console.WriteLine("got to model valid check");
                Console.WriteLine(Item.Id);
                return Page();
            }
            _context.Maintenance.Add(NewMaintenance);
            Item.LastModifiedUtc = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return RedirectToPage("./ItemMaint", new { ItemId = ItemId.ToString() });
        }
    }
}

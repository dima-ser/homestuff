using HomeStuff.Data;
using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace HomeStuff.Pages
{
    public class ItemMaintModel : PageModel
    {
        private readonly SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public int ItemId { get; set; }
        //public List<Models.Maintenance> Maintenances { get; set; } = new List<Models.Maintenance>();
        public List<Models.Maintenance> OverdueMaintenances { get; set; } = new List<Models.Maintenance>();
        public List<Models.Maintenance> UpcomingMaintenances { get; set; } = new List<Models.Maintenance>();
        public List<Models.Maintenance> CompletedMaintenances { get; set; } = new List<Models.Maintenance>();
        public Item? Item;

        [BindProperty]
        public Models.Maintenance NewMaintenance { get; set; } = new Models.Maintenance();
        [BindProperty, DisplayName("Recurrence")]
        public bool NewMaintRecurring { get; set; } = false;
        [BindProperty, DisplayName("Every (days)")]
        public int? NewMaintRecurringDays { get; set; } = 180;
        [BindProperty, DisplayName("# of Occurences")]
        public int? NewMaintNumOccurences { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public string? ErrorMessage { get; set; } = string.Empty;

        public ItemMaintModel(SqliteContext context) 
        { 
            _context = context;
            NewMaintenance.Date = DateOnly.FromDateTime(DateTime.Now);
        }

        public async Task<IActionResult> OnGetAsync(int ItemId)
        {
            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (Item == null)
            {
                return NotFound();
            }
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            OverdueMaintenances = _context.Maintenance.Where(i => i.Item!.Id == ItemId && i.Date < today && i.Completed == false).ToList();
            UpcomingMaintenances = _context.Maintenance.Where(i => i.Item!.Id == ItemId && i.Date >= today && i.Completed == false).ToList();
            CompletedMaintenances = _context.Maintenance.Where(i => i.Item!.Id == ItemId && i.Completed == true).ToList();
            NewMaintenance.ItemId = Item.Id;
            ViewData["Title"] = Item.Name;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (NewMaintenance == null || Item == null)
            {
                return Page();
            }
            //NewMaintenance.Item = Item;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!NewMaintenance.Completed && NewMaintRecurring) 
            {
                if (NewMaintRecurringDays == null || NewMaintNumOccurences == null)
                {
                    return RedirectToPage("./ItemMaint", new { itemid = ItemId.ToString() , errormessage = "Please provide recurrence days and number of occurences" });
                }
                else
                {
                    //List<Models.Maintenance> schedule = new List<Models.Maintenance>((int)NewMaintNumOccurences);
                    //Models.Maintenance maintenance = NewMaintenance;
                    //schedule.Add(maintenance);
                    for (int i = 0; i < NewMaintNumOccurences; i++)
                    {
                        Models.Maintenance maintenance = new()
                        {
                            ItemId = NewMaintenance.ItemId,
                            Date = NewMaintenance.Date.AddDays(i * (int)NewMaintRecurringDays),
                            Description = NewMaintenance.Description,
                            Completed = NewMaintenance.Completed
                        };
                        _context.Maintenance.Add(maintenance);
                    }
                }
            }
            else
                _context.Maintenance.Add(NewMaintenance);
            Item.LastModifiedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToPage("./ItemMaint", new { itemid = ItemId.ToString() });
        }
    }
}

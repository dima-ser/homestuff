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
    public class MaintenanceModel : PageModel
    {
        private readonly SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public int ItemId { get; set; }
        public List<Models.Maintenance> ScheduledMaintenances { get; set; } = new List<Models.Maintenance>();
        public List<Models.Maintenance> CompletedMaintenances { get; set; } = new List<Models.Maintenance>();
        public Item? Item { get; set; }

        [BindProperty]
        public Models.Maintenance NewMaintenance { get; set; } = new Models.Maintenance();
        [BindProperty, DisplayName("Recurrence")]
        public bool NewMaintIsRecurring { get; set; } = false;
        [BindProperty, DisplayName("Every # days")]
        public int? NewMaintRecurrenceFrequency { get; set; } = 180;
        [BindProperty, DisplayName("for # years")]
        public int? NewMaintRecurrenceDuration { get; set; } = 2;
        [BindProperty(SupportsGet = true)]
        public string? ErrorMessage { get; set; } = string.Empty;

        public MaintenanceModel(SqliteContext context) 
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
            ScheduledMaintenances = _context.Maintenance.OrderBy(i => i.Date).Where(i => i.Item!.Id == ItemId && i.Completed == false).ToList();
            CompletedMaintenances = _context.Maintenance.OrderByDescending(i => i.Date).Where(i => i.Item!.Id == ItemId && i.Completed == true).ToList();
            NewMaintenance.ItemId = Item.Id;
            ViewData["Title"] = Item.Name;
            return Page();
        }

        public async Task<IActionResult> OnPostToggleCompleteAsync(int? id)
        {
            Models.Maintenance maintenance = _context.Maintenance.FirstOrDefault(i => i.Id == id)!;
            Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId)!;
            if (maintenance == null || Item == null)
            {
                return NotFound();
            }
            maintenance.Completed = !maintenance.Completed;
            Item.LastModifiedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Maintenance", new { itemid = maintenance.ItemId.ToString() });
        }

        //public async Task<IActionResult> OnPostUncompleteAsync(int? id)
        //{
        //    Models.Maintenance maintenance = _context.Maintenance.FirstOrDefault(i => i.Id == id)!;
        //    Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId)!;
        //    if (maintenance == null || Item == null)
        //    {
        //        return NotFound();
        //    }
        //    maintenance.Completed = false;
        //    Item.LastModifiedUtc = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();
        //    return RedirectToPage("./Maintenance", new { itemid = maintenance.ItemId.ToString() });
        //}

        public async Task<IActionResult> OnPostAddAsync()
        {

            Item = _context.Item.FirstOrDefault(i => i.Id == ItemId)!;
            if (NewMaintenance == null || Item == null)
            {
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!NewMaintenance.Completed && NewMaintIsRecurring) 
            {
                if (NewMaintRecurrenceFrequency == null || NewMaintRecurrenceDuration == null)
                {
                    return RedirectToPage("./Maintenance", new { itemid = ItemId.ToString() , errormessage = "Please provide recurrence frequency and duration" });
                }
                else
                {
                    DateOnly maxDate = NewMaintenance.Date.AddYears((int)NewMaintRecurrenceDuration);
                    DateOnly occurenceDate = NewMaintenance.Date;
                    //int i = 0;
                    while (occurenceDate <= maxDate)
                    {
                        Models.Maintenance maintenance = new()
                        {
                            ItemId = NewMaintenance.ItemId,
                            Date = occurenceDate,
                            Description = NewMaintenance.Description,
                            Completed = NewMaintenance.Completed
                        };
                        _context.Maintenance.Add(maintenance);
                        occurenceDate = occurenceDate.AddDays((int)NewMaintRecurrenceFrequency);
                    } 
                }
            }
            else
                _context.Maintenance.Add(NewMaintenance);
            Item.LastModifiedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Maintenance", new { itemid = ItemId.ToString() });
        }
    }
}

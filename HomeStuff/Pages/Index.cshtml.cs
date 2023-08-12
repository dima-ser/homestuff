using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeStuff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.SqliteContext _context;
        public IList<Models.Item> RecentItems { get; set; } = default!;
        public IList<Models.Maintenance> OverdueMaintenance { get; set; } = default!;
        public IList<Models.Maintenance> UpcomingMaintenance { get; set; } = default!;

        public IndexModel(Data.SqliteContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            RecentItems = _context.Item.OrderByDescending(x => x.LastModifiedUtc).Take(6).ToList();
            foreach (var item in RecentItems)
            {
                item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
            }
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            OverdueMaintenance = _context.Maintenance.Where(x => x.Completed == false && x.Date <= today).OrderBy(x => x.Date).ToList();
            foreach (var maintenance in OverdueMaintenance)
            {
                maintenance.Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId);
            }
            UpcomingMaintenance = _context.Maintenance.Where(x => x.Completed == false && x.Date > today && x.Date <= today.AddDays(7)).OrderBy(x => x.Date).ToList();
            foreach (var maintenance in UpcomingMaintenance)
            {
                maintenance.Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId);
            }
        }
    }
}

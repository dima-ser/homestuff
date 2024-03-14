using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HomeStuff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.SqliteContext _context;
        public IList<Models.Item> RecentItems { get; set; } = default!;
        //public IList<Models.Maintenance> OverdueMaintenance { get; set; } = default!;
        public IList<Models.Maintenance> UpcomingMaintenance { get; set; } = default!;
        public IList<Models.Maintenance> CompletedMaintenance { get; set; } = default!;
        public const int UPCOMING_MAINTENANCE_DAYS = 14;
        public const int COMPLETED_MAINTENANCE_DAYS = 14;

        [DataType(DataType.Currency)]
        public double? TotalValue = 0;
        public List<Location> ValueLocations {  get; set; } = new List<Location>();
        public List<double?> ValueNumbers { get; set; } = new List<double?>();

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
            //OverdueMaintenance = _context.Maintenance.Where(x => x.Completed == false && x.Date <= today).OrderBy(x => x.Date).ToList();
            //foreach (var maintenance in OverdueMaintenance)
            //{
            //    maintenance.Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId);
            //}
            UpcomingMaintenance = _context.Maintenance.Where(x => x.Completed == false && x.Date <= today.AddDays(UPCOMING_MAINTENANCE_DAYS)).OrderBy(x => x.Date).ToList();
            foreach (var maintenance in UpcomingMaintenance)
            {
                maintenance.Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId);
            }
            CompletedMaintenance = _context.Maintenance.Where(x => x.Completed == true && x.Date <= today && x.Date >= today.AddDays(-COMPLETED_MAINTENANCE_DAYS)).OrderByDescending(x => x.Date).ToList();
            foreach (var maintenance in CompletedMaintenance)
            {
                maintenance.Item = _context.Item.FirstOrDefault(i => i.Id == maintenance.ItemId);
            }

            TotalValue = _context.Item.Sum(i => i.PurchasePrice);
            foreach (var location in _context.Location)
            {
                ValueLocations.Add(location);
                ValueNumbers.Add(_context.Item.Where(i => i.LocationId == location.Id).Sum(i => i.PurchasePrice));
            }
        }
    }
}

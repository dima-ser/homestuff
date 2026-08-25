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
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public IList<Models.Item> RecentlyUpdatedItems { get; set; } = new List<Models.Item>();
        public IList<Models.Item> RecentlyViewedItems { get; set; } = new List<Models.Item>();
        //public IList<Models.Maintenance> OverdueMaintenance { get; set; } = default!;
        public IList<Models.Maintenance> UpcomingMaintenance { get; set; } = new List<Models.Maintenance>();
        public IList<Models.Maintenance> CompletedMaintenance { get; set; } = new List<Models.Maintenance>();
        public const int UPCOMING_MAINTENANCE_DAYS = 14;
        public const int COMPLETED_MAINTENANCE_DAYS = 14;

        [DataType(DataType.Currency)]
        public double? TotalValue = 0;
        public List<Location> ValueLocations {  get; set; } = new List<Location>();
        public List<double?> ValueNumbers { get; set; } = new List<double?>();
        public IList<Models.Item> MissingItems { get; set; } = new List<Models.Item>();

        public IndexModel(Data.SqliteContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.Configuration = configuration;
            this.WebHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
            RecentlyUpdatedItems = _context.Item.OrderByDescending(x => x.LastModifiedUtc).Take(6).ToList();
            foreach (var item in RecentlyUpdatedItems)
            {
                // why do I have to do this for Sets, but not for Locations?
                item.ItemSet = _context.ItemSet.FirstOrDefault(l => l.Id == item.ItemSetId);
                //item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
            }
            RecentlyViewedItems = _context.Item.OrderByDescending(x => x.LastViewedUtc).Take(6).ToList();
            foreach (var item in RecentlyViewedItems)
            {
                // why do I have to do this for Sets, but not for Locations?
                item.ItemSet = _context.ItemSet.FirstOrDefault(l => l.Id == item.ItemSetId);
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

            TotalValue = _context.Item.Where(i => i.Status == Item.ItemStatus.Active).Sum(i => i.PurchasePrice);
            foreach (var location in _context.Location.OrderBy(i => i.FullName))
            {
                ValueLocations.Add(location);
                double? sublocationsTotal = 0;
                foreach (var sublocation in _context.Location.Where(l=>l.ParentId== location.Id))
                {
                    sublocationsTotal += _context.Item.Where(i => i.LocationId == sublocation.Id && i.Status == Item.ItemStatus.Active).Sum(i => i.PurchasePrice);
                }
                ValueNumbers.Add(_context.Item.Where(i => i.LocationId == location.Id && i.Status == Item.ItemStatus.Active).Sum(i => i.PurchasePrice) + sublocationsTotal);
            }
            MissingItems = _context.Item.Where(i=>i.Status == Item.ItemStatus.Missing).OrderBy(i => i.Name).ToList();
            //foreach (var item in MissingItems)
            //{
            //    item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
            //}
        }
    }
}

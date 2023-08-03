using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeStuff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeStuff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HomeStuff.Data.SqliteContext _context;
        public IList<HomeStuff.Models.Item> Items { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? q { get; set; }
        public SelectList? Locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? l { get; set; }

        public IndexModel(ILogger<IndexModel> logger, HomeStuff.Data.SqliteContext context)
        {
            _logger = logger;
            _context = context;
            Locations = new SelectList(context.Location, nameof(Location.Id), nameof(Location.Name));
        }


        public void OnGet()
        {
            if (_context.Item != null)
            {
                var items = from i in _context.Item select i;
                if (!string.IsNullOrEmpty(q))
                {
                    items = items.Where(s => s.Name.ToLower().Contains(q.ToLower()) || 
                    (s.Description != null && s.Description.ToLower().Contains(q.ToLower())) ||
                    (s.Manufacturer != null && s.Manufacturer.ToLower().Contains(q.ToLower())) ||
                    (s.ModelNumber != null && s.ModelNumber.ToLower().Contains(q.ToLower())) ||
                    (s.SerialNumber != null && s.SerialNumber.ToLower().Contains(q.ToLower())) ||
                    (s.Vendor != null && s.Vendor.ToLower().Contains(q.ToLower())) ||
                    (s.SKU != null && s.SKU.ToLower().Contains(q.ToLower())));
                }
                if (!string.IsNullOrEmpty(l))
                {
                    items = items.Where(i => i.LocationId.ToString() == l);
                }

                Items = items.OrderByDescending(i => i.LastModifiedUtc).ToList();
                foreach (var item in Items)
                {
                    item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
                }
            }
        }

        public void OnPostSearch()
        {

        }
    }
}
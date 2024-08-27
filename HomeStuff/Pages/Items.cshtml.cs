using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeStuff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.AccessControl;
using static HomeStuff.Models.Item;

namespace HomeStuff.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public IList<HomeStuff.Models.Item> Items { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }
        public SelectList? Locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? LocationId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ItemSetId { get; set; }
        public SelectList? ItemStatuses { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public double? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public const int PAGE_SIZE = 48;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PAGE_SIZE));
        private readonly IItemService _itemService;

        public ItemsModel(HomeStuff.Data.SqliteContext context, IItemService itemService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _itemService = itemService;
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;

            Locations = new SelectList(context.Location.OrderBy(l=>l.FullName), nameof(Location.Id), nameof(Location.FullName));
            var statuses = from ItemStatus d in Enum.GetValues(typeof(Item.ItemStatus))
                             select new { ID = (int)d, Name = d.ToString() };
            ItemStatuses = new SelectList(statuses, "ID", "Name");
        }


        public async Task OnGetAsync()
        {
            if (_context.Item != null)
            {
                Items = await _itemService.GetPaginatedResult(CurrentPage, Query, LocationId, MinPrice, Status, PAGE_SIZE);
                Count = await _itemService.GetCount(Query, LocationId, MinPrice, Status);
            }
        }

    }
}
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
        public int Count { get; set; } // total number of items matching the filter criteria
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        private readonly IItemService _itemService;

        // query string parameters
        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }
        public SelectList? Locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? LocationId { get; set; }
        public SelectList? ItemSets { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ItemSetId { get; set; }
        public SelectList? ItemStatuses { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public double? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public ItemsModel(HomeStuff.Data.SqliteContext context, IItemService itemService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _itemService = itemService;
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
            PageSize = Config.GetItemPageSize(configuration);

            Locations = new SelectList(context.Location.OrderBy(l=>l.FullName), nameof(Location.Id), nameof(Location.FullName));
            var statuses = from ItemStatus d in Enum.GetValues(typeof(Item.ItemStatus))
                             select new { ID = (int)d, Name = d.ToString() };
            ItemStatuses = new SelectList(statuses, "ID", "Name");
            ItemSets = new SelectList(context.ItemSet.OrderBy(l => l.Name), nameof(ItemSet.Id), nameof(ItemSet.Name));
        }


        public async Task OnGetAsync()
        {
            if (_context.Item != null)
            {
                Items = await _itemService.GetPaginatedResult(CurrentPage, Query, LocationId, MinPrice, Status, ItemSetId, PageSize);
                Count = await _itemService.GetCount(Query, LocationId, MinPrice, Status, ItemSetId);
            }
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeStuff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeStuff.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public IList<HomeStuff.Models.Item> Items { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? q { get; set; }
        public SelectList? Locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? l { get; set; }
        [BindProperty(SupportsGet = true)]
        public double? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public const int PAGE_SIZE = 48;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PAGE_SIZE));
        private readonly IItemService _itemService;

        public ItemsModel(HomeStuff.Data.SqliteContext context, IItemService itemService)
        {
            _context = context;
            _itemService = itemService;
            Locations = new SelectList(context.Location.OrderBy(i => i.Name), nameof(Location.Id), nameof(Location.Name));
        }


        public async Task OnGetAsync()
        {
            if (_context.Item != null)
            {
                Items = await _itemService.GetPaginatedResult(CurrentPage, q, l, MinPrice, PAGE_SIZE);
                Count = await _itemService.GetCount(q, l, MinPrice);

               
            }
        }

    }
}
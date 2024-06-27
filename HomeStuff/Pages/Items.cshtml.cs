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
        public string? q { get; set; }
        public SelectList? Locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? l { get; set; }
        public SelectList? ItemStatuses { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? s { get; set; }
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

            //var query = from l1 in context.Location
            //            join l2 in context.Location on l1.ParentId equals l2.Id 
            //            into gj
            //            from subgroup in gj.DefaultIfEmpty()
            //            orderby subgroup.Name, l1.Name
            //            select new { l1.Id, Breadcrumb = subgroup.Name != null ? $"{subgroup.Name} -> {l1.Name}" : l1.Name };
            //FormattableString rawSql = $"select l1.Id, case when l2.Name is not null then l2.Name || '->' || l1.Name else l1.Name end as Name, null as ParentId from Location l1 left join Location l2 on l1.ParentId=l2.Id order by case when l2.Name is not null then l2.Name || '->' || l1.Name else l1.Name end";
            //Locations = new SelectList(context.Location.FromSql(rawSql), nameof(Location.Id), nameof(Location.Name));
            Locations = new SelectList(context.Location.OrderBy(l=>l.FullName), nameof(Location.Id), nameof(Location.FullName));
            var statuses = from ItemStatus d in Enum.GetValues(typeof(Item.ItemStatus))
                             select new { ID = (int)d, Name = d.ToString() };
            ItemStatuses = new SelectList(statuses, "ID", "Name");
        }


        public async Task OnGetAsync()
        {
            if (_context.Item != null)
            {
                Items = await _itemService.GetPaginatedResult(CurrentPage, q, l, MinPrice, s, PAGE_SIZE);
                Count = await _itemService.GetCount(q, l, MinPrice, s);

               
            }
        }

    }
}
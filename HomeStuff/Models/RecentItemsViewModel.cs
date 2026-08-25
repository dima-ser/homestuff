using Microsoft.AspNetCore.Mvc.RazorPages;


namespace HomeStuff.Models
{
    public class RecentItemsViewModel : PageModel
    {
        public string Title {get; set; } = "";
        public IList<Item> Items {get; set;} = new List<Item>();

        public required IConfiguration Configuration { get; set; }
        public required IWebHostEnvironment WebHostEnvironment { get; set; }
    }
}
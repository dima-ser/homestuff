using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeStuff.Data;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class SetsModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public SetsModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public IList<ItemSet> ItemSets { get;set; } = default!;
        public bool[] HasItems { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.ItemSet != null)
            {
                ItemSets = await _context.ItemSet.OrderBy(i=>i.Name).ToListAsync();
                HasItems = new bool[ItemSets.Count];
                for (int i = 0; i < ItemSets.Count; i++)
                {
                    ItemSets[i].Location = _context.Location.Where(l => l.Id == ItemSets[i].LocationId).FirstOrDefault();
                    HasItems[i] = _context.Item.Where(m => m.ItemSetId == ItemSets[i].Id).Any();
                }
            }
        }
    }
}

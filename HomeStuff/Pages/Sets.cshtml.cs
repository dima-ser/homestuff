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

        public IList<ItemSet> ItemSet { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ItemSet != null)
            {
                ItemSet = await _context.ItemSet.ToListAsync();
            }
        }
    }
}

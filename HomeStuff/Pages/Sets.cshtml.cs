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

        public IList<ItemSet> Sets { get;set; } = default!;
        public async void OnGet()
        {
            if (_context.ItemSet != null)
            {
                Sets = await _context.ItemSet.OrderBy(i => i.Name).ToListAsync();
            }
        }
    }
}

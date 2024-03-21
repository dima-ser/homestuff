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
    public class LocationsModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;

        public LocationsModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public IList<Location> RootLocations { get;set; } = default!;
        public List<Location>[] Sublocations { get;set; } = default!;
        public async void OnGet()
        {
            if (_context.Location != null)
            {
                RootLocations =  _context.Location.Where(i=>i.ParentId==null).OrderBy(i => i.Name).ToList();
                Sublocations = new List<Location>[RootLocations.Count];
                for (int i = 0; i < RootLocations.Count; i++)
                {
                    Sublocations[i] = new List<Location>();
                    if (_context.Location.Where(l => l.ParentId == RootLocations[i].Id).Any())
                        Sublocations[i] =  _context.Location.Where(l => l.ParentId == RootLocations[i].Id).OrderBy(l => l.Name).ToList();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeStuff.Data;
using HomeStuff.Models;

namespace HomeStuff.Pages
{
    public class SetNewModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public string? RedirectUrl { get; set; } = null;

        public SetNewModel(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemSet ItemSet { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.ItemSet == null || ItemSet == null)
            {
                return Page();
            }

            _context.ItemSet.Add(ItemSet);
            await _context.SaveChangesAsync();
            if (string.IsNullOrEmpty(RedirectUrl))
                return RedirectToPage("./Sets");
            else
                return Redirect(RedirectUrl);
        }
    }
}

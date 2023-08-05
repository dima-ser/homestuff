using CsvHelper;
using CsvHelper.Configuration;
using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;


namespace HomeStuff.Pages
{
    public class ExportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HomeStuff.Data.SqliteContext _context;
        [BindProperty(SupportsGet = true)]
        public string? run { get; set; }

        public ExportModel(ILogger<IndexModel> logger, Data.SqliteContext context)
        {
            _logger = logger;
            _context = context;
        }

        public ActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(run))
            {
                List<ItemImport> itemsForExport = new List<ItemImport>();
                foreach (var item in _context.Item)
                {
                    ItemImport itemImport = new ItemImport
                    {
                        Name = item.Name,
                        Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId)!.Name,
                        Description = item.Description,
                        Manufacturer = item.Manufacturer,
                        ModelNumber = item.ModelNumber,
                        SerialNumber = item.SerialNumber,
                        PurchasePrice = item.PurchasePrice,
                        Vendor = item.Vendor,
                        PurchaseDate = item.PurchaseDate,
                        SKU = item.SKU
                    };
                    itemsForExport.Add(itemImport);
                }

                var filePath = Path.GetTempFileName();
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(itemsForExport);
                }

                return File(System.IO.File.OpenRead(filePath), "application/octet-stream", "homestuff-exported-items.csv");
            }
            else
                return Page();
        }
    }
}

using CsvHelper;
using CsvHelper.Configuration;
using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;


namespace HomeStuff.Pages
{
    public class ExportModel : PageModel
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public bool ExportItems { get; set; }
        [BindProperty]
        public bool ExportMaintenance { get; set; }
        [BindProperty]
        public bool MaintCompletedOnly { get; set; }
        //[BindProperty(SupportsGet = true)]
        //public bool IncludeAttachmentUrls { get; set; }
        [BindProperty]
        public bool ActiveItemsOnly { get; set; }

        public ExportModel(Data.SqliteContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = environment;
            _configuration = configuration;
        }

        public ActionResult OnPost()
        {

            if (ExportItems)
            {
                
                List<ItemExport> itemsForExport = new();
                foreach (var item in _context.Item)
                {
                    if (item.Status == Item.ItemStatus.Active || !ActiveItemsOnly)
                    {                    
                        ItemExport itemExport = new()
                        {
                            Name = item.Name,
                            Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId)!.FullName,
                            Description = item.Description,
                            Manufacturer = item.Manufacturer,
                            ModelNumber = item.ModelNumber,
                            SerialNumber = item.SerialNumber,
                            PurchasePrice = item.PurchasePrice,
                            Vendor = item.Vendor,
                            PurchaseDate = item.PurchaseDate,
                            SKU = item.SKU,
                            HasAttachments = Item.HasAttachments(_webHostEnvironment, _configuration, item.Id),
                            ItemUrl = Url.PageLink("Item", null, new { id = item.Id}),
                            Status = item.Status.ToString()
                        };
                        itemsForExport.Add(itemExport);
                    }
                }

                var filePath = Path.GetTempFileName();
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap(new ItemExportMap());
                    csv.WriteRecords(itemsForExport);
                }
                return File(System.IO.File.OpenRead(filePath), "application/octet-stream", "homestuff-exported-items.csv");
            }
            else if (ExportMaintenance)
            {
                List<MaintenanceExport> maintenanceForExport = new();
                foreach (var maint in _context.Maintenance)
                {
                    if (maint.Completed || (!maint.Completed && !MaintCompletedOnly))
                    {
                        var item = _context.Item.Where(i => i.Id == maint.ItemId).FirstOrDefault();
                        MaintenanceExport maintenanceExport = new()
                        {
                            
                            ItemName = item != null ? item.Name : "Item no longer in the database",
                            MaintenanceDescription = maint.Description,
                            Date = maint.Date,
                            Completed = maint.Completed ? "Yes" : "No"
                        };
                        maintenanceForExport.Add(maintenanceExport);
                    }
                }

                var filePath = Path.GetTempFileName();
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap(new MaintenanceExportMap());
                    csv.WriteRecords(maintenanceForExport);
                }
                return File(System.IO.File.OpenRead(filePath), "application/octet-stream", "homestuff-exported-maintenance.csv");
            }
            else
                return Page();
        }
    }
}

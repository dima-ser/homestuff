using Azure.Core;
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

        [BindProperty(SupportsGet = true)]
        public string? Run { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IncludeAttachmentUrls { get; set; }

        public ExportModel(Data.SqliteContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = environment;
            _configuration = configuration;
        }

        public ActionResult OnGet()
        {
            //Console.WriteLine(UriHelper.BuildAbsolute(Request.Scheme, Request.Host));
            //Console.WriteLine(Url.Link("Index", new { itemid = "test", name = "test" }));
            Console.WriteLine(Url.PageLink("ItemAttachment", null, new { itemid = "test", name = "test" }));
            //Console.WriteLine(Request.Scheme+ Request.Host);
            if (!string.IsNullOrEmpty(Run))
            {
                
                List<ItemExport> itemsForExport = new();
                foreach (var item in _context.Item)
                {
                    ItemExport itemExport = new()
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
                        SKU = item.SKU,
                        HasAttachments = Item.HasAttachments(_webHostEnvironment, _configuration, item.Id),
                        ItemUrl = Url.PageLink("Item", null, new { id = item.Id})
                    };

                    
                    //if (IncludeAttachmentUrls)
                    //{
                    //    itemExport.AttachmentUrls = new List<string>();
                    //    List<ItemAttachment> attachments = Item.GetAttachments(_webHostEnvironment, _configuration, this, item.Id);
                    //    if (attachments.Count > 0)
                    //    {
                    //        foreach (var attachment in attachments)
                    //        {
                    //            string fullUrl = Url.PageLink("ItemAttachment", null, new { itemid = item.Id, name = attachment.Name });
                    //            itemExport.AttachmentUrls.Add(fullUrl);
                    //        }
                    //    }
                    //    if (itemExport.AttachmentUrls != null)
                    //        Console.WriteLine(itemExport.AttachmentUrls.ToString());
                    //}
                    itemsForExport.Add(itemExport);
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
            else
                return Page();
        }
    }
}

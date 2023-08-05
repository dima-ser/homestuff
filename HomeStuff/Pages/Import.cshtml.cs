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
using System.Globalization;

namespace HomeStuff.Pages
{
    public class ImportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HomeStuff.Data.SqliteContext _context;
        public IFormFile ImportFile { get; set; }
        public string? ImportError { get; set; }
        public bool ImportSuccess = false;
        public ImportModel(ILogger<IndexModel> logger, Data.SqliteContext context)
        {
            _logger = logger;
            _context = context;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (ImportFile != null)
            {
                if (ImportFile.Length > 0)
                {
                    try
                    {
                        var filePath = Path.GetTempFileName();

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            ImportFile.CopyTo(stream);
                        }
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                        };
                        
                        using (var reader = new StreamReader(filePath))
                        using (var csv = new CsvReader(reader, config))
                        {
                            csv.Context.RegisterClassMap(new ItemImportMap());
                            var importItems = csv.GetRecords<ItemImport>().ToList();
                            
                            // check that all Names and Locations are non-empty
                            int i = 1;
                            Console.WriteLine("Validation started, count is " + importItems.Count);
                            foreach (ItemImport importItem in importItems)
                            {
                                if (string.IsNullOrEmpty(importItem.Name.Trim()) || string.IsNullOrEmpty(importItem.Location.Trim()))
                                {
                                    ImportError = "Empty Name or Location on line " + (i + 1).ToString();
                                    break;
                                }
                                i++;
                            }

                            Console.WriteLine("In between, count is " + importItems.Count);
                            if (string.IsNullOrEmpty(ImportError))
                            {
                                // validation succeeded, proceed with import
                                Console.WriteLine("Import started, count is " + importItems.Count);
                                foreach (ItemImport importItem in importItems)
                                {
                                    Console.WriteLine("Importing " + importItem.Name);
                                    //Models.Item item = default!;
                                    string itemName = importItem.Name;
                                    if (_context.Location.FirstOrDefault(l => l.Name == importItem.Location.Trim()) == null)
                                    {
                                        Console.WriteLine("Adding location " + importItem.Location.Trim());
                                        _context.Location.Add(new Location { Name = importItem.Location.Trim() });
                                        _context.SaveChanges();
                                    }
                                    Location itemLocation = _context.Location.FirstOrDefault(l => l.Name == importItem.Location.Trim())!;
                                    Models.Item item = new Models.Item { Name = itemName, Location = itemLocation, LocationId = itemLocation.Id };
                                    //string? itemDescription, itemManufacturer, itemModelNumber, itemSerialNumber, itemVendor, itemSKU;
                                    //DateOnly? itemPurchaseDate;
                                    //double? itemPurchasePrice;
                                    if (!string.IsNullOrEmpty(importItem.Description))
                                        item.Description = importItem.Description;
                                    if (!string.IsNullOrEmpty(importItem.Manufacturer))
                                        item.Manufacturer = importItem.Manufacturer;
                                    if (!string.IsNullOrEmpty(importItem.ModelNumber))
                                        item.ModelNumber = importItem.ModelNumber;
                                    if (!string.IsNullOrEmpty(importItem.SerialNumber))
                                        item.SerialNumber = importItem.SerialNumber;
                                    if (importItem.PurchasePrice != null)
                                        item.PurchasePrice = importItem.PurchasePrice;
                                    if (!string.IsNullOrEmpty(importItem.Vendor))
                                        item.Vendor = importItem.Vendor;
                                    if (importItem.PurchaseDate != null)
                                        item.PurchaseDate = importItem.PurchaseDate;
                                    if (!string.IsNullOrEmpty(importItem.SKU))
                                        item.SKU = importItem.SKU;
                                    item.LastModifiedUtc = DateTime.UtcNow;
                                    
                                    _context.Item.Add(item);
                                    Console.WriteLine("Done with " + importItem.Name);
                                }
                                _context.SaveChanges();
                                Console.WriteLine("Saved changes ");
                                this.ImportSuccess = true;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        throw;
                        //ImportError = ex.Message;
                    }
                   
                }
            }
            

        }
    }
}

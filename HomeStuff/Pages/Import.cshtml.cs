using CsvHelper;
using CsvHelper.Configuration;
using HomeStuff.Migrations;
using HomeStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HomeStuff.Pages
{
    public class ImportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HomeStuff.Data.SqliteContext _context;
        public IFormFile ImportFile { get; set; }
        public string? ImportValidationError { get; set; }
        public bool ImportValidationSuccess = false;
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
                            var items = csv.GetRecords<ItemImport>();
                            // check that all Names and Locations are non-empty
                            int i = 1;
                            foreach (var item in items)
                            {
                                if (string.IsNullOrEmpty(item.Name.Trim()) || string.IsNullOrEmpty(item.Location.Trim()))
                                {
                                    ImportValidationError = "Empty Name or Location on line " + (i + 1).ToString();
                                    break;
                                }
                                i++;
                            }
                            if (string.IsNullOrEmpty(ImportValidationError))
                            {
                                this.ImportValidationSuccess = true;

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ImportValidationError = ex.Message;
                    }
                   
                }
            }
            

        }
    }
}

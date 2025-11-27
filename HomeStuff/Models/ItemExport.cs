using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeStuff.Models
{
    public class ItemExport
    {
        public required string Name { get; set; }
        public required string Location {  get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public string? ModelNumber { get; set; }
        public string? SerialNumber { get; set; }
        public double? PurchasePrice { get; set; }
        public string? Vendor { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public string? SKU { get; set; }
        //public List<string>? AttachmentUrls { get; set; }
        public bool HasAttachments { get; set; }
        public string? ItemUrl { get; set; }
        public string? Status { get; set; }
    }
    public sealed class ItemExportMap : ClassMap<ItemExport>
    {
        public ItemExportMap()
        {
            Map(m => m.Name);
            Map(m => m.Location);
            Map(m => m.Description).Optional();
            Map(m => m.Manufacturer).Optional();
            Map(m => m.ModelNumber).Optional();
            Map(m => m.SerialNumber).Optional();
            Map(m => m.PurchasePrice).Optional();
            Map(m => m.Vendor).Optional();
            Map(m => m.PurchaseDate).Optional();
            Map(m => m.SKU).Optional();
            //Map(m => m.AttachmentUrls).Optional().Convert(row => string.Join(", ", row.Value.AttachmentUrls));
            Map(m => m.HasAttachments);
            Map(m => m.ItemUrl);
            Map(m => m.Status);
        }
    }

}

using CsvHelper.Configuration;

namespace HomeStuff.Models
{
    public class MaintenanceExport
    {
        public required string ItemName { get; set; }
        public required string MaintenanceDescription { get; set; }
        public required DateOnly Date { get; set; }
        public required string Completed { get; set; }
    }

    public sealed class MaintenanceExportMap : ClassMap<MaintenanceExport>
    {
        public MaintenanceExportMap()
        {
            Map(m => m.ItemName);
            Map(m => m.MaintenanceDescription);
            Map(m => m.Date);
            Map(m => m.Completed);
        }
    }

}

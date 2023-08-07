namespace HomeStuff.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string PhysicalPath { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public Attachment(string physicalPath, string url, string name)
        {
            this.PhysicalPath = physicalPath;
            this.Url = url;
            this.Name = name;
        }   
    }
}

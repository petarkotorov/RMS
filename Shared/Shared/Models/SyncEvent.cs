namespace Shared.Models
{
    public class SyncEvent
    {
        public string Operation { get; set; }

        public ProductModel Product { get; set; }

        public string SourceStore { get; set; }

        public string DestinationStore { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}

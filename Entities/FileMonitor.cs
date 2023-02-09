namespace Entities
{
    public class FileMonitor
    {
        public long SourceFileId { get; set; }
        public DateTime? TimeMonitored { get; set; }
        public byte[] FileCopied { get; set; }
        public DateTime TimeCopied { get; set; }
    }
}

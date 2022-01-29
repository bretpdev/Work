
namespace COQTSKBLDR
{
    public class FileProcessingRecord
    {
        public int FileProcessingId { get; set; }
        public string LineData { get; set; }
        public int LineDataId { get; set; }

        public QueueBuilderRecord ParsedLineData { get; set; }
    }
}
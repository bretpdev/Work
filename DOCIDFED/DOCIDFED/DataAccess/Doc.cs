    namespace DOCIDFED
{
    public class Doc
    {
        public enum Source
        {
            BU,
            CF,
            PO
        }

        public int MappingId { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string Arc { get; set; }
        public string OriginalARC { get; set; }
        public Source DocSource { get; set; }
        public bool CreateQueue { get; set; }
        public bool QueueCreated { get; set; }
        public bool AddTd22 { get; set; }
        public bool BU { get; set; }
        public bool PO { get; set; }
        public static Doc Empty => new Doc() { MappingId = 0, DocId = "", DocType = "", Arc = "" };
    }
}
namespace PMTLNHIST
{
    public class BorrowerData
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public double Principal { get; set; }
        public double Interest { get; set; }
        public double LegalCosts { get; set; }
        public double OtherCosts { get; set; }
        public double CollectionCosts { get; set; }
        public double ProjectedCollectionCosts { get; set; }
    }
}
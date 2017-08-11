namespace Core.Model
{
    public class TaxSlabDetail
    {
        public int Id { get; set; }
        public int TaxSlabId { get; set; }
        public int? SlabFromAmount { get; set; }
        public int? SlabToAmount { get; set; }
        public int Percentage { get; set; }
    }
}

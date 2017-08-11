namespace Core.Model
{
    public class TaxSlab
    {
        public int Id { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string Category { get; set; }
    }
}

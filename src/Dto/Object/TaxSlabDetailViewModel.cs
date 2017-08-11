namespace Dto.Object
{
    public class TaxSlabDetailViewModel
    {
        public int Id { get; set; }

        public int? SlabFromAmount { get; set; }

        public int? SlabToAmount { get; set; }

        public string SSlabFromAmount => SlabFromAmount.HasValue ? SlabFromAmount.ToString() : "";

        public string SSlabToAmount => SlabToAmount.HasValue ? SlabToAmount.ToString() : "";

        public int Percentage { get; set; }

        public string Slab
        {
            get
            {
                if (!SlabFromAmount.HasValue && SlabToAmount.HasValue)
                    return string.Format("Income upto Rs. {0}", SlabToAmount);

                if (SlabFromAmount.HasValue && SlabToAmount.HasValue)
                    return string.Format("Income between Rs. {0} - {1}", SlabFromAmount, SlabToAmount);

                if (SlabFromAmount.HasValue && !SlabToAmount.HasValue)
                    return string.Format("Income above Rs. {0}", SlabFromAmount);

                return null;
            }
        }
    }
}
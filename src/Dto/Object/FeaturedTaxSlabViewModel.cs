using System.Collections.Generic;

namespace Dto.Object
{
    public class FeaturedTaxSlabViewModel
    {
        public FeaturedTaxSlabViewModel()
        {
            this.TaxSlabs = new List<TaxSlabViewModel>();
        }

        public IList<TaxSlabViewModel> TaxSlabs { get; }
    }
}

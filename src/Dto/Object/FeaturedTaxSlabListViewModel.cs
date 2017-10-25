using System.Collections.Generic;

namespace Dto.Object
{
    public class FeaturedTaxSlabListViewModel
    {
        private readonly List<TaxSlabViewModel> _taxSlabs;

        public FeaturedTaxSlabListViewModel()
        {
            this._taxSlabs = new List<TaxSlabViewModel>();
        }

        public IList<TaxSlabViewModel> TaxSlabs => this._taxSlabs;
    }
}

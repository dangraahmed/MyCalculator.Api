using System.Collections.Generic;

namespace Dto.Object
{
    public class FeaturedTaxSlabDetailListViewModel
    {
        private readonly List<TaxSlabDetailViewModel> _taxSlabDetail;

        public FeaturedTaxSlabDetailListViewModel()
        {
            this._taxSlabDetail = new List<TaxSlabDetailViewModel>();
        }

        public IList<TaxSlabDetailViewModel> TaxSlabDetail => this._taxSlabDetail;
    }
}

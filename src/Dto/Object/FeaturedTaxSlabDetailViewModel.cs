using System.Collections.Generic;

namespace Dto.Object
{
    public class FeaturedTaxSlabDetailViewModel
    {
        private List<TaxSlabDetailViewModel> taxSlabDetail;

        public FeaturedTaxSlabDetailViewModel()
        {
            taxSlabDetail = new List<TaxSlabDetailViewModel>();
        }

        public IList<TaxSlabDetailViewModel> TaxSlabDetail => taxSlabDetail;

        public TaxSlabViewModel TaxSlab { get; set; }
    }
}

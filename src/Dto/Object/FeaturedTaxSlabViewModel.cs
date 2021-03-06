﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dto.Object
{
    public class FeaturedTaxSlabViewModel : TaxSlabViewModel
    {
        private readonly List<TaxSlabDetailViewModel> _taxSlabDetail;

        public FeaturedTaxSlabViewModel()
        {
            this._taxSlabDetail = new List<TaxSlabDetailViewModel>();
        }

        public IList<TaxSlabDetailViewModel> TaxSlabDetail => this._taxSlabDetail;
    }
}

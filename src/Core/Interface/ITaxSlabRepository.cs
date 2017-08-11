using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;

namespace Core.Interface
{
    public interface ITaxSlabRepository
    {
        IEnumerable<TaxSlab> GetTaxSlabs();
        IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId);
        bool InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails);
        bool DeleteTaxSlab(int taxSlabId);
    }
}

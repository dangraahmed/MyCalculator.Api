using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;

namespace BusinessLogic
{
    public class TaxSlabBL : ITaxSlabBL
    {
        private readonly ITaxSlabRepository _repository;

        public TaxSlabBL(ITaxSlabRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {
            return _repository.GetTaxSlabs();
        }

        public IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId)
        {
            return _repository.GetTaxSlabDetail(taxSlabId);
        }

        public int InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            return _repository.InsertUpdateTaxSlab(taxSlab, taxSlabDetails);
        }

        public bool DeleteTaxSlab(int taxSlabId)
        {
            if (CanDelete())
            {
                return _repository.DeleteTaxSlab(taxSlabId);
            }
            else
            {
                return false;
            }
            
        }

        #region Private Method
        private bool CanDelete()
        {
            return true;
        }
        #endregion
    }
}

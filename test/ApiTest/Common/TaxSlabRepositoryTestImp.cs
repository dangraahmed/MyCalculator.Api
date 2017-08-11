using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;

namespace ApiTest.Common
{
    public class TaxSlabRepositoryTestImp : ITaxSlabRepository
    {
        public List<TaxSlab> TaxSlabTestData { get; set; }

        public List<TaxSlabDetail> TaxSlabDetailTestData { get; set; }

        public TaxSlabRepositoryTestImp()
        {
            TaxSlabTestData = GetTaxSlabsTestData();
            TaxSlabDetailTestData = GetTaxSlabDetailTestData();
        }

        #region Interface ITaxSlabRepository
        public bool DeleteTaxSlab(int taxSlabId)
        {
            try
            {
                TaxSlabTestData.RemoveAll(tax => tax.Id == taxSlabId);
                TaxSlabDetailTestData.RemoveAll(tax => tax.TaxSlabId == taxSlabId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId)
        {
            return TaxSlabDetailTestData.Where(tax => tax.TaxSlabId == taxSlabId);
        }

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {
            return TaxSlabTestData;
        }
                
        public bool InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region private method
        private List<TaxSlab> GetTaxSlabsTestData()
        {
            return new List<TaxSlab>()
            {
                new TaxSlab() {Id = 1, FromYear = 2001, ToYear = 2002, Category = "A"},
                new TaxSlab() {Id = 2, FromYear = 2002, ToYear = 2003, Category = "B"},
                new TaxSlab() {Id = 3, FromYear = 2003, ToYear = 2004, Category = "C"},
                new TaxSlab() {Id = 4, FromYear = 2004, ToYear = 2005, Category = "D"}
            };
        }

        private List<TaxSlabDetail> GetTaxSlabDetailTestData()
        {
            return new List<TaxSlabDetail>()
            {
                new TaxSlabDetail() {Id = 1,TaxSlabId= 1, SlabToAmount = 100, Percentage = 110},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 1, SlabFromAmount = 101, SlabToAmount = 200, Percentage = 120},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 1, SlabFromAmount = 201, SlabToAmount = 300, Percentage = 130},
                new TaxSlabDetail() {Id = 4,TaxSlabId= 1, SlabFromAmount = 301, Percentage = 140},

                new TaxSlabDetail() {Id = 1,TaxSlabId= 2, SlabToAmount = 100, Percentage = 210},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 2, SlabFromAmount = 101, SlabToAmount = 200, Percentage = 220},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 2, SlabFromAmount = 201, SlabToAmount = 300, Percentage = 230},
                new TaxSlabDetail() {Id = 4,TaxSlabId= 2, SlabFromAmount = 301, SlabToAmount = 400, Percentage = 240},
                new TaxSlabDetail() {Id = 5,TaxSlabId= 2, SlabFromAmount = 401, Percentage = 250},

                new TaxSlabDetail() {Id = 1,TaxSlabId= 3, SlabToAmount = 100, Percentage = 310},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 3, SlabFromAmount = 101, SlabToAmount = 200, Percentage = 320},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 3, SlabFromAmount = 201, SlabToAmount = 300, Percentage = 330},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 3, SlabFromAmount = 301, SlabToAmount = 400, Percentage = 340},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 3, SlabFromAmount = 401, SlabToAmount = 500, Percentage = 350},
                new TaxSlabDetail() {Id = 4,TaxSlabId= 3, SlabFromAmount = 501, Percentage = 360},

                new TaxSlabDetail() {Id = 1,TaxSlabId= 4, SlabToAmount = 100, Percentage = 410},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 4, SlabFromAmount = 101, SlabToAmount = 200, Percentage = 420},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 4, SlabFromAmount = 201, SlabToAmount = 300, Percentage = 430},
                new TaxSlabDetail() {Id = 2,TaxSlabId= 4, SlabFromAmount = 301, SlabToAmount = 400, Percentage = 440},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 4, SlabFromAmount = 401, SlabToAmount = 500, Percentage = 450},
                new TaxSlabDetail() {Id = 3,TaxSlabId= 4, SlabFromAmount = 501, SlabToAmount = 600, Percentage = 460},
                new TaxSlabDetail() {Id = 4,TaxSlabId= 4, SlabFromAmount = 601, Percentage = 470},
            };
        }
        #endregion
    }
}

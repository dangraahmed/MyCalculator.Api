using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;
using MongoDataAccess.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoDataAccess.Repository
{
    public class TaxSlabRepository : ITaxSlabRepository
    {
        private readonly MongoDBContext _mongoDBContext;

        public TaxSlabRepository(string connectionString)
        {
            _mongoDBContext = new MongoDBContext(connectionString, "my_calculator", true);
        }

        public bool DeleteTaxSlab(int taxSlabId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId)
        {
            var taxSlabDetail = new List<TaxSlabDetail>(); // TODO : Auto mapper need to implemented

            foreach (var employee in _mongoDBContext.TaxSlabDetails.AsQueryable().Where(x => x.TaxSlabId == taxSlabId))
            {
                taxSlabDetail.Add(new TaxSlabDetail()
                {
                    Id = employee.Id,
                    TaxSlabId = employee.TaxSlabId,
                    SlabFromAmount = employee.SlabFromAmount,
                    SlabToAmount = employee.SlabToAmount,
                    Percentage = employee.Percentage
                });
            }

            return taxSlabDetail;
        }

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {

            var taxSlabs = new List<TaxSlab>(); // TODO : Auto mapper need to implemented

            foreach (var employee in _mongoDBContext.TaxSlab.AsQueryable())
            {
                taxSlabs.Add(new TaxSlab()
                {
                    Id = employee.Id,
                    FromYear = employee.FromYear,
                    ToYear = employee.ToYear,
                    Category = employee.Category
                });
            }

            return taxSlabs;
        }

        public int InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            if (taxSlab.Id == -1)
            {
                var lastTaxSlab = _mongoDBContext.TaxSlab.Aggregate().SortByDescending((a) => a.Id).FirstAsync();
                var mdTaxSlab = new MdTaxSlab()
                {
                    Id = Convert.ToInt32(lastTaxSlab.Id) + 1,
                    FromYear = taxSlab.FromYear,
                    ToYear = taxSlab.ToYear,
                    Category = taxSlab.Category
                };

                _mongoDBContext.TaxSlab.InsertOneAsync(mdTaxSlab);

                var lastTaxSlabDetails = _mongoDBContext.TaxSlabDetails.Aggregate().SortByDescending((a) => a.Id).FirstAsync();
                int index = 0;
                foreach (var taxSlabDetail in taxSlabDetails)
                {
                    var mdTaxSlabDetail = new MdTaxSlabDetail()
                    {
                        Id = lastTaxSlabDetails.Id + index++,
                        TaxSlabId = mdTaxSlab.Id,
                        Percentage = taxSlabDetail.Percentage,
                        SlabFromAmount = taxSlabDetail.SlabFromAmount,
                        SlabToAmount = taxSlabDetail.SlabToAmount
                    };

                    _mongoDBContext.TaxSlabDetails.InsertOneAsync(mdTaxSlabDetail);
                }
            }

            return 0;
        }
    }
}

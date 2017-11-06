using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;
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
            throw new NotImplementedException();
        }

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {
            try
            {
                var query = from e in _mongoDBContext.TaxSlab.AsQueryable<TTaxSlab>()
                    select e;

                foreach (var employee in query)
                {
                    // process employees named "John"
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public int InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            throw new NotImplementedException();
        }
    }
}

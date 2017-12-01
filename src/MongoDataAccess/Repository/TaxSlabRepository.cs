using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Interface;
using Core.Model;
using MongoDataAccess.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoDataAccess.Repository
{
    public class TaxSlabRepository : ITaxSlabRepository
    {
        private readonly MongoDBContext _mongoDbContext;
        private IMongoQueryable<MdTaxSlabDetail> TaxSlabDetails => _mongoDbContext.TaxSlabDetails.AsQueryable();
        private IMongoQueryable<MdTaxSlab> TaxSlab => _mongoDbContext.TaxSlab.AsQueryable();
        private static IMapper Mapper => AutoMapperInit.Configuration();

        public TaxSlabRepository(string connectionString)
        {
            _mongoDbContext = new MongoDBContext(connectionString, "my_calculator", true);
        }

        public bool DeleteTaxSlab(int taxSlabId)
        {
            try
            {
                _mongoDbContext.TaxSlab.DeleteOne(a => a.Id == taxSlabId);
                _mongoDbContext.TaxSlabDetails.DeleteMany(a => a.TaxSlabId == taxSlabId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId)
        {
            var taxSlabDetail = new List<TaxSlabDetail>();
            foreach (var employee in this.TaxSlabDetails.Where(x => x.TaxSlabId == taxSlabId))
            {
                taxSlabDetail.Add(Mapper.Map<TaxSlabDetail>(employee));
            }

            return taxSlabDetail;
        }

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {
            var taxSlabs = new List<TaxSlab>();

            foreach (var tax in this.TaxSlab)
            {
                taxSlabs.Add(Mapper.Map<TaxSlab>(tax));
            }

            return taxSlabs;
        }

        public int InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            return taxSlab.Id == -1 ? InsertTaxSlab(taxSlab, taxSlabDetails) : UpdateTaxSlab(taxSlab, taxSlabDetails);
        }

        private int InsertTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            // taxSlab insertion
            var taxSlabId = this.TaxSlab.OrderByDescending(a => a.Id).FirstOrDefault()?.Id ?? 0;
            var mdTaxSlab = Mapper.Map<MdTaxSlab>(taxSlab);

            mdTaxSlab.Id = Convert.ToInt32(taxSlabId) + 1;
            _mongoDbContext.TaxSlab.InsertOneAsync(mdTaxSlab);

            // taxSlabDetails insertion
            var taxSlabDetailsId = this.TaxSlabDetails.OrderByDescending(a => a.Id).FirstOrDefault()?.Id ?? 0;
            var index = 1;
            foreach (var taxSlabDetail in taxSlabDetails)
            {
                var mdTaxSlabDetail = Mapper.Map<MdTaxSlabDetail>(taxSlabDetail);

                mdTaxSlabDetail.Id = taxSlabDetailsId + index++;
                mdTaxSlabDetail.TaxSlabId = mdTaxSlab.Id;
                _mongoDbContext.TaxSlabDetails.InsertOneAsync(mdTaxSlabDetail);
            }

            return mdTaxSlab.Id;
        }

        private int UpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            // first update taxSlab
            var mdTaxSlab = Mapper.Map<MdTaxSlab>(taxSlab);
            mdTaxSlab._id = new ObjectId(this.TaxSlab.FirstOrDefault(x => x.Id == taxSlab.Id)._id.ToString());

            _mongoDbContext.TaxSlab.ReplaceOneAsync(s => s.Id == mdTaxSlab.Id, mdTaxSlab);


            // delete taxSlabDetails from database which are not passed by user
            var slabDetails = taxSlabDetails as TaxSlabDetail[] ?? taxSlabDetails.ToArray();
            var ids = new HashSet<int>(slabDetails.Select(x => x.Id));
            var results = this.TaxSlabDetails.Where(x => !ids.Contains(x.Id) && x.TaxSlabId == mdTaxSlab.Id);

            foreach (var mdTaxSlabDetail in results)
            {
                _mongoDbContext.TaxSlabDetails.DeleteOne(det => det.Id == mdTaxSlabDetail.Id);
            }

            // taxSlabDetails update or insert.
            var taxSlabDetailsId = this.TaxSlabDetails.OrderByDescending(a => a.Id).FirstOrDefault()?.Id ?? 0;
            var index = 1;
            foreach (var taxSlabDetail in slabDetails)
            {
                if (taxSlabDetail.Id == -1)
                {
                    // Insert new entry in database
                    var mdTaxSlabDetail = Mapper.Map<MdTaxSlabDetail>(taxSlabDetail);
                    mdTaxSlabDetail.Id = taxSlabDetailsId + index++;
                    mdTaxSlabDetail.TaxSlabId = mdTaxSlab.Id;

                    _mongoDbContext.TaxSlabDetails.InsertOneAsync(mdTaxSlabDetail);
                }
                else if (this.TaxSlabDetails.Any(det => det.Id == taxSlabDetail.Id))
                {
                    // taxSlabDetail exist in database so update it
                    var mdTaxSlabDetail = Mapper.Map<MdTaxSlabDetail>(taxSlabDetail);
                    mdTaxSlabDetail._id = new ObjectId(this.TaxSlabDetails.FirstOrDefault(x => x.Id == taxSlabDetail.Id)._id.ToString());
                    mdTaxSlabDetail.TaxSlabId = mdTaxSlab.Id;

                    _mongoDbContext.TaxSlabDetails.ReplaceOne(s => s.Id == mdTaxSlabDetail.Id, mdTaxSlabDetail);
                }
            }

            return mdTaxSlab.Id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly MongoDBContext _mongoDBContext;

        public TaxSlabRepository(string connectionString)
        {
            _mongoDBContext = new MongoDBContext(connectionString, "my_calculator", true);
        }

        public bool DeleteTaxSlab(int taxSlabId)
        {
            try
            {
                _mongoDBContext.TaxSlab.DeleteOne(a => a.Id == taxSlabId);
                _mongoDBContext.TaxSlabDetails.DeleteMany(a => a.TaxSlabId == taxSlabId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

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
            return taxSlab.Id == -1 ? InsertTaxSlab(taxSlab, taxSlabDetails) : UpdateTaxSlab(taxSlab, taxSlabDetails);
        }

        private int InsertTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            var query = _mongoDBContext.TaxSlab.AsQueryable().OrderByDescending(a => a.Id).FirstOrDefault();
            var mdTaxSlab = new MdTaxSlab()
            {
                Id = Convert.ToInt32(query.Id) + 1,
                FromYear = taxSlab.FromYear,
                ToYear = taxSlab.ToYear,
                Category = taxSlab.Category
            };

            _mongoDBContext.TaxSlab.InsertOneAsync(mdTaxSlab);

            var lastTaxSlabDetails = _mongoDBContext.TaxSlabDetails.AsQueryable().OrderByDescending(a => a.Id).FirstOrDefault();
            var index = 0;
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

            return mdTaxSlab.Id;
        }

        private int UpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {

            var mdTaxSlab = new MdTaxSlab()
            {
                _id = new ObjectId(_mongoDBContext.TaxSlab.AsQueryable().FirstOrDefault(x => x.Id == taxSlab.Id)._id.ToString()),
                Id = taxSlab.Id,
                FromYear = taxSlab.FromYear,
                ToYear = taxSlab.ToYear,
                Category = taxSlab.Category
            };

            _mongoDBContext.TaxSlab.ReplaceOneAsync(s => s.Id == mdTaxSlab.Id, mdTaxSlab);

            var slabDetails = taxSlabDetails as TaxSlabDetail[] ?? taxSlabDetails.ToArray();
            var ids = new HashSet<int>(slabDetails.Select(x => x.Id));
            var results = _mongoDBContext.TaxSlabDetails.AsQueryable().Where(x => !ids.Contains(x.Id) && x.TaxSlabId == mdTaxSlab.Id);

            foreach (var mdTaxSlabDetail in results)
            {
                _mongoDBContext.TaxSlabDetails.DeleteOne(det => det.Id == mdTaxSlabDetail.Id);
            }

            var lastTaxSlabDetails = _mongoDBContext.TaxSlabDetails.AsQueryable().OrderByDescending(a => a.Id).FirstOrDefault();
            var index = 1;
            foreach (var taxSlabDetail in slabDetails)
            {
                if (taxSlabDetail.Id == -1)
                {
                    // Insert new entry in database

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
                else if (_mongoDBContext.TaxSlabDetails.AsQueryable().Any(det => det.Id == taxSlabDetail.Id))
                {
                    // taxSlabDetail exist in database so update it

                    var mdTaxSlabDetail = new MdTaxSlabDetail()
                    {
                        _id = new ObjectId(_mongoDBContext.TaxSlabDetails.AsQueryable().FirstOrDefault(x => x.Id == taxSlabDetail.Id)._id.ToString()),
                        Id = taxSlabDetail.Id,
                        TaxSlabId = mdTaxSlab.Id,
                        Percentage = taxSlabDetail.Percentage,
                        SlabFromAmount = taxSlabDetail.SlabFromAmount,
                        SlabToAmount = taxSlabDetail.SlabToAmount
                    };

                    _mongoDBContext.TaxSlabDetails.ReplaceOne(s => s.Id == mdTaxSlabDetail.Id, mdTaxSlabDetail);
                }
                //else
                //{
                //    // taxSlabDetail don't exist in database so delete it
                //    _mongoDBContext.TaxSlabDetails.DeleteOne(det => det.Id == taxSlabDetail.Id);
                //}
            }

            return mdTaxSlab.Id;
        }
    }
}

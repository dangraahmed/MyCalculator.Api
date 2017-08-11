using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;
using SQLDataAccess.Common;

namespace SQLDataAccess.Repository
{
    public class TaxSlabRepository : ITaxSlabRepository
    {
        private readonly string _connectionString;

        public TaxSlabRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public TaxSlabRepository()
        //{
        //    _connectionString = "Data Source=rav-dsk-414;Initial Catalog=MyCalculator;Persist Security Info=True;User ID=sa;Password=Pa55word";
        //}

        public IEnumerable<TaxSlab> GetTaxSlabs()
        {
            var taxSlabs = new List<TaxSlab>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.CommandText = "proc_tax_slab_read";
                    command.Connection = connection;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taxSlabs.Add(new TaxSlab()
                            {
                                Id = Convert.ToInt32(reader["id"].ToString()),
                                FromYear = Convert.ToInt32(reader["from_year"].ToString()),
                                ToYear = Convert.ToInt32(reader["to_year"].ToString()),
                                Category = reader["category"].ToString()
                            });
                        }
                    }
                }
            }

            return taxSlabs;
        }

        public bool DeleteTaxSlab(int taxSlabId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.CommandText = "proc_tax_slab_delete";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tax_slab_id", taxSlabId);

                        var reader = command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<TaxSlabDetail> GetTaxSlabDetail(int taxSlabId)
        {
            var taxSlabDetail = new List<TaxSlabDetail>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.CommandText = "proc_tax_slab_details_read";
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tax_slab_id", taxSlabId);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taxSlabDetail.Add(new TaxSlabDetail()
                            {
                                Id = Convert.ToInt32(reader["id"].ToString()),
                                TaxSlabId = Convert.ToInt32(reader["tax_slab_id"].ToString()),
                                SlabFromAmount =
                                    reader["from_amount"] != DBNull.Value
                                        ? Convert.ToInt32(reader["from_amount"].ToString())
                                        : (int?)null,
                                SlabToAmount =
                                    reader["to_amount"] != DBNull.Value
                                        ? Convert.ToInt32(reader["to_amount"].ToString())
                                        : (int?)null,
                                Percentage = Convert.ToInt32(reader["percentage"].ToString())
                            });
                        }
                    }
                }
            }

            return taxSlabDetail;
        }

        public bool InsertUpdateTaxSlab(TaxSlab taxSlab, IEnumerable<TaxSlabDetail> taxSlabDetails)
        {
            try
            {
                string taxSlabDetail = taxSlabDetails.Serialize();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;

                        if (taxSlab.Id == -1)
                        {
                            command.CommandText = "proc_tax_slab_create";
                        }
                        else
                        {
                            command.CommandText = "proc_tax_slab_update";
                            command.Parameters.AddWithValue("@id", taxSlab.Id);
                        }

                        command.Parameters.AddWithValue("@from_year", taxSlab.FromYear);
                        command.Parameters.AddWithValue("@to_year", taxSlab.ToYear);
                        command.Parameters.AddWithValue("@category", taxSlab.Category);
                        command.Parameters.AddWithValue("@tax_slab_details", taxSlabDetail);

                        var reader = command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

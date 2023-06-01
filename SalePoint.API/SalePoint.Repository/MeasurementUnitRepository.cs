using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Data;

namespace SalePoint.Repository
{
    public class MeasurementUnitRepository : IMeasurementUnitRepository
    {
        private readonly IConfiguration _configuration;

        public MeasurementUnitRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetMeasurementUnit()
        {
            try
            {
                IEnumerable<MeasurementUnit> measurementUnits;

                string query = @"
                                  SELECT 
	                                     MU.[Id] 'Id',
                                         MU.[Name] 'Name',
                                         MU.[Icon] 'Icon',
                                         MU.[CreationDate] 'CreationDate',
	                                     MU.[ModificationDate] 'ModificationDate',
	                                     MU.[DeletionDate] 'DeletionDate',
										 MU.[IsActive] 'IsActive'
	                                  FROM MeasurementUnit MU
	                                  WHERE MU.IsActive = 1";

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                measurementUnits = await conn.QueryAsync<MeasurementUnit>(query);

                conn.Close();

                return measurementUnits;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
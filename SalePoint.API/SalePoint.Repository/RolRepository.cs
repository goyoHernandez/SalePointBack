using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalePoint.Repository
{
    public class RolRepository : IRolRepository
    {
        private readonly IConfiguration _configuration;
        public RolRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Rol>> GetRols()
        {
            IEnumerable<Rol> rols;
            try
            {
                string query = @"
                                  SELECT 
		                                  [Id],
		                                  [Name],
		                                  [Description],
		                                  [IsActive],
		                                  [CreationDate],
		                                  [ModificationDate],
		                                  [DeletionDate]
		                                FROM Rol
		                                WHERE IsActive = 1";

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                rols = await conn.QueryAsync<Rol>(query);

                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return rols;
        }
    }
}

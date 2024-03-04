using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Data;

namespace SalePoint.Repository
{
    public class DepartmentRepository(IConfiguration configuration) : IDepartmentRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            IEnumerable<Department> departments;

            string query = @"
                                  SELECT 
	                                     D.[Id] 'Id',
                                         D.[Name] 'Name',
                                         D.[CreationDate] 'CreationDate',
	                                     D.[ModificationDate] 'ModificationDate',
	                                     D.[DeletionDate] 'DeletionDate',
										 D.[IsActive] 'IsActive'
	                                  FROM Department D
	                                  WHERE D.IsActive = 1";

            using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
            conn.Open();

            departments = await conn.QueryAsync<Department>(query);

            conn.Close();

            return departments;
        }
    }
}
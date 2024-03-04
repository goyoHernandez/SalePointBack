using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Data;

namespace SalePoint.Repository
{
    public class UserRepository(IConfiguration configuration) : IUserRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<int> CreateUser(StoreUser storeUser)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("name", storeUser.Name);
                parameters.Add("lastName", storeUser.LastName);
                parameters.Add("age", storeUser.Age);
                parameters.Add("address", storeUser.Address);
                parameters.Add("cellphone", storeUser.CellPhone);
                //parameters.Add("avatar", storeUser.Avatar);
                parameters.Add("description", storeUser.Description);
                parameters.Add("rolId", storeUser.RolId);
                parameters.Add("userName", storeUser.UserName);
                parameters.Add("pass", storeUser.Pass);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int userId = await conn.QuerySingleOrDefaultAsync<int>("CreateStoreUser", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return userId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StoreUser>?> GetAllUsers()
        {
            try
            {
                IEnumerable<StoreUser> storeUser;

                string query = @"
                                  SELECT 
		                                  PU.[Id],
		                                  PU.[Name],
		                                  PU.[LastName],
		                                  PU.[Age],
		                                  PU.[Address],
		                                  PU.[CellPhone],
		                                  PU.[Avatar],
		                                  PU.[Description],
		                                  PU.[IsActive],
		                                  PU.[CreationDate],
		                                  PU.[ModificationDate],
		                                  PU.[DeletionDate],
		                                  PU.[RolId],
		                                  PU.[Username],
		                                  PU.[Pass],
		                                  R.[Id],
		                                  R.[Name],
		                                  R.[Description],
		                                  R.[IsActive],
		                                  R.[CreationDate],
		                                  R.[ModificationDate],
		                                  R.[DeletionDate]
		                                FROM StoreUser PU
		                                INNER JOIN Rol R ON PU.RolId = R.Id
		                                WHERE PU.IsActive = 1";

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                storeUser = (await conn.QueryAsync<StoreUser, Rol, StoreUser>(query,
                     map: (pu, r) =>
                     {
                         pu.Rol = r;
                         return pu;
                     },
                     splitOn: "Id")).ToList();

                conn.Close();
                return storeUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StoreUser?> GetUserById(int userId)
        {
            try
            {
                StoreUser? storeUser = new();

                string query = @"
                                  SELECT 
		                                  PU.[Id],
		                                  PU.[Name],
		                                  PU.[LastName],
		                                  PU.[Age],
		                                  PU.[Address],
		                                  PU.[CellPhone],
		                                  PU.[Avatar],
		                                  PU.[Description],
		                                  PU.[IsActive],
		                                  PU.[CreationDate],
		                                  PU.[ModificationDate],
		                                  PU.[DeletionDate],
		                                  PU.[RolId],
		                                  PU.[Username],
		                                  PU.[Pass],
		                                  R.[Id],
		                                  R.[Name],
		                                  R.[Description],
		                                  R.[IsActive],
		                                  R.[CreationDate],
		                                  R.[ModificationDate],
		                                  R.[DeletionDate]
		                                FROM StoreUser PU
		                                INNER JOIN Rol R ON PU.RolId = R.Id
		                                WHERE PU.Id = @userId
		                                AND PU.IsActive = 1";

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                storeUser = (await conn.QueryAsync<StoreUser, Rol, StoreUser?>(query,
                     map: (pu, r) =>
                     {
                         pu.Rol = r;
                         return pu;
                     },
                     splitOn: "Id",
                     param: new { userId })).FirstOrDefault();

                conn.Close();
                return storeUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StoreUser?> Login(Access access)
        {
            try
            {
                StoreUser? storeUser = new();

                string query = @"
                                  SELECT 
		                                  PU.[Id],
		                                  PU.[Name],
		                                  PU.[LastName],
		                                  PU.[Age],
		                                  PU.[Address],
		                                  PU.[CellPhone],
		                                  PU.[Avatar],
		                                  PU.[Description],
		                                  PU.[IsActive],
		                                  PU.[CreationDate],
		                                  PU.[ModificationDate],
		                                  PU.[DeletionDate],
		                                  PU.[RolId],
		                                  PU.[Username],
		                                  PU.[Pass],
		                                  R.[Id],
		                                  R.[Name],
		                                  R.[Description],
		                                  R.[IsActive],
		                                  R.[CreationDate],
		                                  R.[ModificationDate],
		                                  R.[DeletionDate]
		                                FROM StoreUser PU
		                                INNER JOIN Rol R ON PU.RolId = R.Id
		                                WHERE PU.UserName = @userName 
		                                AND PU.Pass = @pass
		                                AND PU.IsActive = 1";

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                storeUser = (await conn.QueryAsync<StoreUser, Rol, StoreUser?>(query,
                     map: (pu, r) =>
                     {
                         pu.Rol = r;
                         return pu;
                     },
                     splitOn: "Id",
                     param: new { access.UserName, access.Pass })).FirstOrDefault();

                conn.Close();
                return storeUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateUser(StoreUser storeUser)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("id", storeUser.Id);
                parameters.Add("name", storeUser.Name);
                parameters.Add("lastName", storeUser.LastName);
                parameters.Add("age", storeUser.Age);
                parameters.Add("address", storeUser.Address);
                parameters.Add("cellphone", storeUser.CellPhone);
                //parameters.Add("avatar", storeUser.Avatar);
                parameters.Add("description", storeUser.Description);
                parameters.Add("rolId", storeUser.RolId);
                parameters.Add("userName", storeUser.UserName);
                parameters.Add("pass", storeUser.Pass);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int userId = await conn.QuerySingleOrDefaultAsync<int>("UpdateStoreUser", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return userId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUserById(int userId)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("userId", userId);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int id = await conn.QuerySingleOrDefaultAsync<int>("DeleteStoreUser", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
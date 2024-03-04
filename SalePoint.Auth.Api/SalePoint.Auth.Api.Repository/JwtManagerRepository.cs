using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalePoint.Auth.Api.Primitives.Interfaces;
using SalePoint.Auth.Api.Primitives.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalePoint.Auth.Api.Repository
{
    public class JwtManagerRepository(IConfiguration configuration) : IJwtManagerRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<TokenAuth?> Authenticate(Access access)
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

            if (storeUser != null)
                return GetToken(storeUser);

            return null;
        }

        private TokenAuth GetToken(StoreUser storeUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimTypes.Sid, storeUser.Id.ToString()),
                        new(ClaimTypes.Name, storeUser.Name),
                        new(ClaimTypes.Role, storeUser.Rol.Name)
                    }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenAuth { Token = tokenHandler.WriteToken(token) };
        }
    }
}
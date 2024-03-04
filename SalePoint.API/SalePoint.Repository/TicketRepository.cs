using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Data;

namespace SalePoint.Repository
{
    public class TicketRepository(IConfiguration configuration) : ITicketRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task CreateTicket(Ticket ticket)
        {
            string sqlStatement = @"DELETE FROM Ticket;
                                    INSERT INTO Ticket VALUES (@companyName, @Address, @Footer, GETDATE(), NULL)";

            using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
            conn.Open();

            await conn.QueryAsync(sqlStatement, param: new
            {
                ticket.CompanyName,
                ticket.Address,
                ticket.Footer
            });
            conn.Close();
        }

        public async Task<Ticket?> GetTicket()
        {
            string query = @"
                                  SELECT 
		                                  [Id],
		                                  [CompanyName],
		                                  [Address],
		                                  [Footer],
		                                  [CreateDate],
		                                  [ModifiedDate]
		                                FROM Ticket";

            using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
            conn.Open();
            Ticket? ticket = await conn.QueryFirstOrDefaultAsync<Ticket?>(query);
            conn.Close();

            return ticket;
        }
    }
}
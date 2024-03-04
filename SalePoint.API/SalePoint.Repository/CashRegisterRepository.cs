using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Data;

namespace SalePoint.Repository
{
    public class CashRegisterRepository(IConfiguration configuration) : ICashRegisterRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<int> ApplyCashFlows(CashFlows cashFlows)
        {
            try
            {
                int idCashWithdrawal = 0;
                DynamicParameters parameters = new();
                parameters.Add("boxCutId", cashFlows.BoxCutId);
                parameters.Add("cashFlowsTypesId", cashFlows.CashFlowsTypesId);
                parameters.Add("quantity", cashFlows.Quantity);
                parameters.Add("reason", cashFlows.Reason);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                idCashWithdrawal = await conn.QueryFirstAsync<int>("ApplyCashFlows", param: parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return idCashWithdrawal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CloseCashRegister(CashRegister cashRegister)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("cashRegisterId", cashRegister.Id);
                parameters.Add("boxcloseReasonId", cashRegister.BoxCloseReasonId);
                parameters.Add("userId", cashRegister.UserId);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int cashRegisterId = await conn.QuerySingleOrDefaultAsync<int>("CloseCashRegister", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return cashRegisterId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CashRegister>> GetAllCashRegister(int? userId)
        {
            try
            {
                IEnumerable<CashRegister> cashRegisters;

                DynamicParameters parameters = new();
                parameters.Add("userId", userId);

                using SqlConnection conn = new (_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                cashRegisters = await conn.QueryAsync<CashRegister, StoreUser, BoxCloseReason, CashRegister>("GetCashRegister",
                    (cr, pu, bcr) =>
                    {
                        cr.StoreUser = pu;
                        cr.BoxCloseReason = bcr;
                        return cr;
                    }, splitOn: "Id,Id",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                conn.Close();

                return cashRegisters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CashFlows>> GetCashFlowsDetail(int boxCutId, int cashFlowsType)
        {
            try
            {
                IEnumerable<CashFlows> cashFlows;

                DynamicParameters parameters = new();
                parameters.Add("boxCutId", boxCutId);
                parameters.Add("cashFlowsType", cashFlowsType);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                cashFlows = await conn.QueryAsync<CashFlows>("GetCashFlowsDetail", param: parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return cashFlows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Sale>> GetProductReturnsDetail(int boxCutId)
        {
            try
            {
                IEnumerable<Sale> salesReturns;

                DynamicParameters parameters = new();
                parameters.Add("boxCutId", boxCutId);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                salesReturns = await conn.QueryAsync<Sale>("ShowProductReturnsByBoxCutId", param: parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return salesReturns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> OpenCashRegister(InitialAmount initialAmount)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("UserId", initialAmount.UserId);
                parameters.Add("initialAmount", initialAmount.Mount);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int cashRegisterId = await conn.QuerySingleOrDefaultAsync<int>("OpenCashRegister", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return cashRegisterId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BoxCutOpen?> ValidateBoxCutOpen(int userId, decimal change)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("UserId", userId);
                parameters.Add("CashChange", change);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                BoxCutOpen? boxCutOpen = await conn.QuerySingleOrDefaultAsync<BoxCutOpen?>("ValidateBoxCutOpen", parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return boxCutOpen;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
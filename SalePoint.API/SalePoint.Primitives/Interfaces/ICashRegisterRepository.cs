namespace SalePoint.Primitives.Interfaces
{
    public interface ICashRegisterRepository
    {
        Task<IEnumerable<CashRegister>> GetAllCashRegister(int? userId);

        Task<IEnumerable<CashFlows>> GetCashFlowsDetail(int boxCutId, int cashFlowsType);

        Task<IEnumerable<Sale>> GetProductReturnsDetail(int boxCutId);

        Task<int> OpenCashRegister(InitialAmount initialAmount);

        Task<int> CloseCashRegister(CashRegister cashRegister);

        Task<int> ApplyCashFlows(CashFlows cashFlows);

        Task<BoxCutOpen?> ValidateBoxCutOpen(int userId, decimal change);
    }
}
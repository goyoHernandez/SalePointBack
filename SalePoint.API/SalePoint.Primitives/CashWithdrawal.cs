#nullable disable
namespace SalePoint.Primitives
{
    public record CashWithdrawal
    {
        public int Id { get; set; }
        
        public int BoxCutId { get; set; }
        
        public decimal Amount { get; set; }

        public string Reason { get; set; }
        
        public DateTime? WithdrawalDate { get; set; }
    }
}
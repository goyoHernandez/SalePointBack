#nullable disable
namespace SalePoint.Primitives
{
    public record CashFlowsTypes
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime DeletionDate { get; set; }
    }
}
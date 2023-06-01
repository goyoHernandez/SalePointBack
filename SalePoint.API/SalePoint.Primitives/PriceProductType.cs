namespace SalePoint.Primitives
{
    public record PriceProductType
    {
        public int ProductId { get; set; }
        
        public decimal SalesPrice { get; set; }
        
        public decimal PercentageProfit { get; set; }
        
        public decimal Revenue { get; set; }
        
        public int Wholesale { get; set; }
    }
}
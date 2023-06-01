namespace SalePoint.Primitives
{
#nullable disable
    public record ProductModel
    {
        public int ProductId { get; set; }
        
        public string NameProduct { get; set; }
        
        public string BarCode { get; set; }
        
        public DateTime ExpirationDate { get; set; }
        
        public string Description { get; set; }
        
        public decimal Stock { get; set; }
        
        public decimal MinimumStock { get; set; }
        
        public decimal PurchasePrice { get; set; }
        
        public byte[]? Thumbnail { get; set; }
        
        public int UnitMeasureId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public int UserId { get; set; }
        
        public decimal SalesPrice1 { get; set; }
        
        public decimal PercentageProfit1 { get; set; }
        
        public decimal Revenue1 { get; set; }
        
        public decimal Wholesale1 { get; set; }
        
        public decimal SalesPrice2 { get; set; }
        
        public decimal PercentageProfit2 { get; set; }
        
        public decimal Revenue2 { get; set; }
        
        public decimal Wholesale2 { get; set; }
        
        public int DeparmentId { get; set; }
        
        public string DeparmentName { get; set; }
        
        public string MeasurementUnitName { get; set; }
        
        public string Icon { get; set; }
    }
}
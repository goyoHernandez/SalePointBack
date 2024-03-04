namespace SalePoint.BulkLoad.API.Primitives
{
    public class NewProductType
    {
        public string? Name { get; set; }

        public string? BarCode { get; set; }
        
        public string? ExpirationDate { get; set; }
        
        public string? Description { get; set; }
        
        public string? Stock { get; set; }

        public string? MinimumStock { get; set; }

        public string? PurchasePrice { get; set; }

        public string? UnitMeasureId { get; set; }

        public string? UserId { get; set; }

        public string? DepartmentName { get; set; }  
        
        public string? RetailSalePrice { get; set; }

        public string? WholeSalePrice { get; set; }
        
        public string? WholeSaleQuantity { get; set; }
    }
}
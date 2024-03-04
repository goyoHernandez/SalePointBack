namespace SalePoint.BulkLoad.API.Primitives
{
    public  class UpgradeProductType
    {
        public string Name { get; set; } = string.Empty;

        public string? ExpirationDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? Stock { get; set; }

        public string? PurchasePrice { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? RetailSalePrice { get; set; }

        public string? WholeSalePrice { get; set; }

        public string? WholeSaleQuantity { get; set; }
    }
}

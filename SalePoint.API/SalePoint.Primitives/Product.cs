using System.Diagnostics.Metrics;

namespace SalePoint.Primitives
{
#nullable enable
    public record Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? BarCode { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string? Description { get; set; }

        public decimal Stock { get; set; }

        public decimal MinimumStock { get; set; }

        public decimal PurchasePrice { get; set; }

        public byte[]? Thumbnail { get; set; }

        public int UnitMeasureId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public DateTime? DeletionDate { get; set; }

        public int UserId { get; set; }

        public Department? Department { get; set; }

        public ProductDepartment? ProductDepartment { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }

        public List<PriceProduct>? PriceProducts { get; set; }      
        
        public Product()
        {
            PriceProducts = new List<PriceProduct>();
        }
    }
}
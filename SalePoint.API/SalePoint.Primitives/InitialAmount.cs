namespace SalePoint.Primitives
{
    public record InitialAmount
    {
        public int UserId { get; set; }

        public decimal Mount { get; set; }
    }
}

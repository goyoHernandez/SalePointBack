namespace SalePoint.Primitives
{
    public record Reimbursement
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public int BoxCutId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public decimal? Gain { get; set; }

        public decimal UnitPrice { get; set; }

        public int UserId { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
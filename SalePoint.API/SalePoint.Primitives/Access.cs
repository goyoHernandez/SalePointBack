namespace SalePoint.Primitives
{
    public record Access
    {
        public required string UserName { get; set; }

        public required string Pass { get; set; }
    }
}
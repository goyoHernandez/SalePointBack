namespace SalePoint.Auth.Api.Primitives.Models
{
#nullable disable
    public record Access
    {
        public string UserName { get; set; }

        public string Pass { get; set; }
    }
}
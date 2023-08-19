namespace SalePoint.Auth.Api.Primitives.Models
{
#nullable disable
    public class TokenAuth
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}

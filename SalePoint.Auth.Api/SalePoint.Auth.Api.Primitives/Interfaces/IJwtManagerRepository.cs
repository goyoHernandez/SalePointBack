using SalePoint.Auth.Api.Primitives.Models;

namespace SalePoint.Auth.Api.Primitives.Interfaces
{
    public interface IJwtManagerRepository
    {
        Task<TokenAuth?> Authenticate(Access access);
    }
}
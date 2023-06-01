namespace SalePoint.Primitives.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetRols();
    }
}
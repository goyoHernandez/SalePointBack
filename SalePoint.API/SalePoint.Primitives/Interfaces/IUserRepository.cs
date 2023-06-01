namespace SalePoint.Primitives.Interfaces
{
    public interface IUserRepository
    {
        Task<int>CreateUser(StoreUser storeUser);

        Task<IEnumerable<StoreUser>?>GetAllUsers();

        Task<StoreUser?>GetUserById(int userId);

        Task<int>UpdateUser(StoreUser storeUser);
        
        Task<int>DeleteUserById(int userId);
        
        Task<StoreUser?>Login(Access access);
    }
}
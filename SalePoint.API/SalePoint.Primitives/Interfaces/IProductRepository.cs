namespace SalePoint.Primitives.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateProduct(Product product);

        Task<ProductModel> GetAllProducts(int pageNumber, int pageSize);

        Task<IEnumerable<Product>> GetProductsExpiringSoon();
        
        Task<IEnumerable<Product>> GetProductsNearCompletition();

        Task<Product> GetProductById(int productId);

        Task<IEnumerable<Product>> GetProductByBarCode(string barCode);

        Task<ProductModel> GetProductByNameOrDescriptionPaginate(string keyWord, int pageNumber, int pageSize);

        Task<ProductModel> GetProductByNameOrDescription(string keyWord);

        Task<int> UpdateProduct(Product product);

        Task<int> UpdateStockProduct(int idProduct, int stock);

        Task<int> DeleteProduct(int id, int userId);

    }
}
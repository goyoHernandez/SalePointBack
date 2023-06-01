using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalePoint.Primitives.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateProduct(Product product);

        Task<IEnumerable<ProductModel>> GetAllProducts();

        Task<IEnumerable<Product>> GetProductsExpiringSoon();
        
        Task<IEnumerable<Product>> GetProductsNearCompletition();

        Task<Product> GetProductById(int productId);

        Task<IEnumerable<Product>> GetProductByBarCode(string barCode);

        Task<IEnumerable<ProductModel>> GetProductByNameOrDescription(string keyWord);

        Task<int> UpdateProduct(Product product);

        Task<int> UpdateStockProduct(int idProduct, int stock);

        Task<int> DeleteProduct(int id, int userId);

    }
}
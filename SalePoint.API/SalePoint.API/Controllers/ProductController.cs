using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            return Json(await _productRepository.CreateProduct(product));
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllProducts()
        {
            return Json(await _productRepository.GetAllProducts());
        }
        
        [HttpGet("ExpiringSoon")]
        public async Task<ActionResult> GetExpiringSoonProducts()
        {
            return Json(await _productRepository.GetProductsExpiringSoon());
        }
        
        [HttpGet("NearCompletition")]
        public async Task<ActionResult> GetProductsNearCompletition()
        {
            return Json(await _productRepository.GetProductsNearCompletition());
        }

        [HttpGet("Get/productId/{productId}")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            return Json(await _productRepository.GetProductById(productId));
        }

        [HttpGet("GetBy/barCode/{barCode}")]
        public async Task<ActionResult> GetProductByBarCode(string barCode)
        {
            return Json(await _productRepository.GetProductByBarCode(barCode));
        }

        [HttpGet("GetBy/NameOrDescription/{keyWord}")]
        public async Task<ActionResult> GetProductByNameOrDescription(string keyWord)
        {
            return Json(await _productRepository.GetProductByNameOrDescription(keyWord));
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            return Json(await _productRepository.UpdateProduct(product));
        }

        [HttpPut("Update/{idProduct}/stock")]
        public async Task<ActionResult> UpdateStockProduct(int idProduct, [FromBody]int stock)
        {
            return Json(await _productRepository.UpdateStockProduct(idProduct, stock));
        }

        [HttpDelete("Delete/id/{id}/userId/{userId}")]
        public async Task<ActionResult> DeleteProduct(int id, int userId)
        {
            return Json(await _productRepository.DeleteProduct(id, userId));
        }
    }
}
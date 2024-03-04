using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IProductRepository productRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

        [HttpPost("Create")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            try
            {
                return Json(await _productRepository.CreateProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("All/pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public async Task<ActionResult> GetAllProducts(int pageNumber, int pageSize)
        {
            try
            {
                return Json(await _productRepository.GetAllProducts(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("ExpiringSoon")]
        public async Task<ActionResult> GetExpiringSoonProducts()
        {
            try
            {
                return Json(await _productRepository.GetProductsExpiringSoon());

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("NearCompletition")]
        public async Task<ActionResult> GetProductsNearCompletition()
        {
            try
            {
                return Json(await _productRepository.GetProductsNearCompletition());

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("Get/productId/{productId}")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            try
            {
                return Json(await _productRepository.GetProductById(productId));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("GetBy/barCode/{barCode}")]
        public async Task<ActionResult> GetProductByBarCode(string barCode)
        {
            try
            {
                return Json(await _productRepository.GetProductByBarCode(barCode));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("GetBy/NameOrDescription/{keyWord}/pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public async Task<ActionResult> GetProductByNameOrDescriptionPaginate(string keyWord, int pageNumber, int pageSize)
        {
            try
            {
                return Json(await _productRepository.GetProductByNameOrDescriptionPaginate(keyWord, pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpGet("GetBy/NameOrDescription/{keyWord}")]
        public async Task<ActionResult> GetProductByNameOrDescription(string keyWord)
        {
            try
            {
                return Json(await _productRepository.GetProductByNameOrDescription(keyWord));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            try
            {
                return Json(await _productRepository.UpdateProduct(product));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpPut("Update/{idProduct}/stock")]
        public async Task<ActionResult> UpdateStockProduct(int idProduct, [FromBody] int stock)
        {
            try
            {
                return Json(await _productRepository.UpdateStockProduct(idProduct, stock));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpDelete("Delete/id/{id}/userId/{userId}")]
        public async Task<ActionResult> DeleteProduct(int id, int userId)
        {
            try
            {
                return Json(await _productRepository.DeleteProduct(id, userId));

            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }
    }
}
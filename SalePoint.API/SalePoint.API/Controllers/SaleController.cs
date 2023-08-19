using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SaleController : Controller
    {
        private ISaleRepository _saleRepository;

        public SaleController(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        }

        [HttpPost("getSales")]
        public async Task<IActionResult> GetSalesByUserId(FilterSaleProducts filterSaleProducts)
        {
            return Ok(await _saleRepository.GetSalesByUserId(filterSaleProducts));
        }

        [HttpPost]
        public async Task<IActionResult> SellItems(List<SellerItemsType> sellerItemsTypes)
        {
            return Ok(await _saleRepository.SellItems(sellerItemsTypes));
        }

        [HttpPost("productReturns")]
        public async Task<IActionResult> ReturnProduct(ProductReturns productReturns)
        {
            return Ok(await _saleRepository.ReturnProduct(productReturns));
        }
    }
}
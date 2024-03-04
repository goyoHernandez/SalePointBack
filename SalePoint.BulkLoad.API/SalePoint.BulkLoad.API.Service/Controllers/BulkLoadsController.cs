using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.BulkLoad.API.Primitives.Interfaces;

namespace SalePoint.BulkLoad.API.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BulkLoadsController(IBulkLoadRepository bulkLoadRepository) : Controller
    {
        private readonly IBulkLoadRepository _bulkLoadRepository = bulkLoadRepository;

        [HttpPost("bulk-load/{userId}/products")]
        public async Task<IActionResult> BulkLoadProducts(IFormFile fileCsv, [FromRoute] int userId)
        {
            if (fileCsv == null || fileCsv.Length == 0)
                return BadRequest("El archivo CSV no fue proporcionado o está vacío.");


            return Json(await _bulkLoadRepository.BulkLoadProducts(fileCsv, userId));
        }

        [HttpPut("bulk-upgrade/{userId}/products")]
        public async Task<IActionResult> UpgradeBulkLoadProducts(IFormFile fileCsv, [FromRoute] int userId)
        {
            if (fileCsv == null || fileCsv.Length == 0)
                return BadRequest("El archivo CSV no fue proporcionado o está vacío.");


            return Json(await _bulkLoadRepository.UpgradeBulkLoadProducts(fileCsv, userId));
        }
    }
}
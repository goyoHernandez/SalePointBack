using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using SalePoint.Repository;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RolController : Controller
    {
        private IRolRepository _rolRepository;

        public RolController(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetRols()
        {
            return Json(await _rolRepository.GetRols());
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MeasurementUnitController : Controller
    {
        private IMeasurementUnitRepository _measureUnitRepository;

        public MeasurementUnitController(IMeasurementUnitRepository measureUnitRepository)
        {
            _measureUnitRepository = measureUnitRepository ?? throw new ArgumentNullException(nameof(measureUnitRepository));
        }

        [HttpGet()]
        public async Task<ActionResult> GetMeasurementUnit()
        {
            return Json(await _measureUnitRepository.GetMeasurementUnit());
        }
    }
}
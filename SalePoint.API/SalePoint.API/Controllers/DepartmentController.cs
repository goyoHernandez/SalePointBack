using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        [HttpGet("Get")]
        public async Task<ActionResult> GetAllDepartments()
        {
            try
            {
                return Json(await _departmentRepository.GetAllDepartments());
            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Auth.Api.Primitives.Interfaces;
using SalePoint.Auth.Api.Primitives.Models;

namespace SalePoint.Auth.Api.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]    
    public class UserController : Controller
    {
        private readonly IJwtManagerRepository _jwtManagerRepository;

        public UserController(IJwtManagerRepository jwtManagerRepository)
        {
            _jwtManagerRepository = jwtManagerRepository ?? throw new ArgumentNullException(nameof(jwtManagerRepository));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Authenticate(Access access)
        {
            TokenAuth? token = await _jwtManagerRepository.Authenticate(access);

            if (token is null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
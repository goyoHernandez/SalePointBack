using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet("Get")]
        public async Task<ActionResult> GetAllUsers()
        {
            return Json(await _userRepository.GetAllUsers());
        }

        [HttpGet("Get/ByUserId/{userId}")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            return Json(await _userRepository.GetUserById(userId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateUser(StoreUser storeUser)
        {
            return Json(await _userRepository.CreateUser(storeUser));
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateUser(StoreUser storeUser)
        {
            return Json(await _userRepository.UpdateUser(storeUser));
        }

        [HttpDelete("Delete/ByUserId/{userId}")]
        public async Task<ActionResult> DeleteUserById(int userId)
        {
            return Json(await _userRepository.DeleteUserById(userId));
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(Access access)
        {
            return Json(await _userRepository.Login(access));
        }
    }
}
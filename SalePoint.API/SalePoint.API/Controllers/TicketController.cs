using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Collections;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetTicket()
        {
            try
            {
                return Json(await _ticketRepository.GetTicket());
            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            try
            {
                await _ticketRepository.CreateTicket(ticket);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { isError = true, message = ex.Message });
            }
        }
    }
}

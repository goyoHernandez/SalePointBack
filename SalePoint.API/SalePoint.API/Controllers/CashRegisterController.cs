using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CashRegisterController : Controller
    {
        private ICashRegisterRepository _cashRegisterRepository;

        public CashRegisterController(ICashRegisterRepository cashRegisterRepository)
        {
            _cashRegisterRepository = cashRegisterRepository ?? throw new ArgumentNullException(nameof(cashRegisterRepository));
        }

        [HttpGet("Get/ByUserId/{userId}")]
        public async Task<ActionResult> GetAllCashRegister(int userId)
        {
            return Json(await _cashRegisterRepository.GetAllCashRegister(userId));
        }

        [HttpGet("Get/cashFlowsDetail/boxCutId/{boxCutId}/cashFlowsType/{cashFlowsType}")]
        public async Task<ActionResult> GetCashFlowsDetail(int boxCutId, int cashFlowsType)
        {
            return Json(await _cashRegisterRepository.GetCashFlowsDetail(boxCutId, cashFlowsType));
        }

        [HttpGet("Get/productReturnsDetail/boxCutId/{boxCutId}")]
        public async Task<ActionResult> GetProductReturnsDetail(int boxCutId)
        {
            return Json(await _cashRegisterRepository.GetProductReturnsDetail(boxCutId));
        }

        [HttpPost("Get/BoxCutOpen/userId/{userId}")]
        public async Task<ActionResult> ValidateBoxCutOpen(int userId, [FromBody] decimal change)
        {
            return Json(await _cashRegisterRepository.ValidateBoxCutOpen(userId, change));
        }

        [HttpPost("Open")]
        public async Task<ActionResult> OpenCashRegister(InitialAmount initialAmount)
        {
            return Json(await _cashRegisterRepository.OpenCashRegister(initialAmount));
        }

        [HttpPost("CashFlow")]
        public async Task<ActionResult> ApplyCashFlows(CashFlows cashFlows)
        {
            return Json(await _cashRegisterRepository.ApplyCashFlows(cashFlows));
        }

        [HttpPut("Close")]
        public async Task<ActionResult> CloseCashRegister(CashRegister cashRegister)
        {
            return Json(await _cashRegisterRepository.CloseCashRegister(cashRegister));
        }
    }
}
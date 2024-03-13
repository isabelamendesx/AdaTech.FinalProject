using Microsoft.AspNetCore.Mvc;
using Model.Service.Services;
using Model.Application.DTO;
using Model.Domain.Entities;
using Model.Application.API.Util;
using Model.Application.DTO.Validators;
using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Identity.Constants;
using Model.Domain.Interfaces;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RefundController : ControllerBase
    {
        private readonly IRefundService _service;

        public RefundController(IRefundService service)
        {
            _service = service;
        }

        [HttpPost]
        [Idempotent(ExpiresInMilliseconds = 10000)]
        public async Task<IActionResult> CreateRefund([FromBody] RefundRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var refund = new Refund()
            {
                Description = request.Description,
                Category = new Category { Name = request.Category},
                Status = EnumParser.ParseStatus(request.Status),
                Total = request.Total
            };

            var createdRefund = await _service.CreateRefund(refund);

            return Ok(createdRefund);
        }

        [HttpGet]
        [Route("{status}")]
        public async Task<ActionResult<IEnumerable<Refund?>>> GetAllByStatus([ValidateStatus] string status)
        {
            var parsedStatus = EnumParser.ParseStatus(status);

            var refunds = await _service.GetAllByStatus(parsedStatus);

            return Ok(refunds);
        }

        [HttpPost]
        [Route("/approve/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> ApproveRefund([FromRoute] uint id, [FromRoute] uint userId)
        {
            var refund = await _service.ApproveRefund(id, userId);
            return Ok(refund);
        }

        [HttpPost]
        [Route("/refuse/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager + "," + Roles.Supervisor)]
        public async Task<IActionResult> RefuseRefund([FromRoute] uint id, [FromRoute] uint userId)
        {
            var refund = await _service.RefuseRefund(id, userId);
            return Ok(refund);
        }

    }
}

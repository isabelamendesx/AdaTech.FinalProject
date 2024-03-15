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
using Microsoft.AspNetCore.Http.Timeouts;
using Serilog;
using Model.Application.API.DTO;

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
        //[Idempotent(ExpiresInMilliseconds = 10000)]
        public async Task<IActionResult> CreateRefund([FromBody] RefundRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid Refund model state: {@ModelState}", ModelState.Values);
                return UnprocessableEntity(ModelState);
            }

            var refund = new Refund()
            {
                Description = request.Description,
                Category = new Category { Id = request.CategoryId},
                Status = EnumParser.ParseStatus(request.Status),
                Total = request.Total
            };

            var createdRefund = await _service.CreateRefund(refund, HttpContext.RequestAborted);
            Log.Information("New Refund Submitted and {@Status} by rule with ID {@RuleId}", createdRefund.Status, createdRefund.Operations.First().ApprovalRule.Id);

            return Ok(createdRefund);
        }

        [HttpGet]
        [Route("{status}")]
        public async Task<ActionResult<IEnumerable<Refund?>>> GetAllByStatus([ValidateStatus] string status,
                                                    [FromQuery] PaginationParametersDTO paginationParameters)
        {
            var parsedStatus = EnumParser.ParseStatus(status);

            var refunds = await _service.GetAllByStatus(parsedStatus, HttpContext.RequestAborted);


            if (paginationParameters.PageNumber == 0 || paginationParameters.PageSize == 0)
                return Ok(refunds);


            var paginatedRefunds = refunds
                                    .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                                    .Take(paginationParameters.PageSize)
                                    .ToList();


            return Ok(paginatedRefunds);
        }

        [HttpPost]
        [Route("/approve/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> ApproveRefund([FromRoute] uint id, [FromRoute] uint userId)
        {
            var refund = await _service.ApproveRefund(id, userId, HttpContext.RequestAborted);
            return Ok(refund);
        }

        [HttpPost]
        [Route("/reject/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager + "," + Roles.Supervisor)]
        public async Task<IActionResult> RejectRefund([FromRoute] uint id, [FromRoute] uint userId)
        {
            var refund = await _service.RejectRefund(id, userId, HttpContext.RequestAborted);
            return Ok(refund);
        }

    }
}

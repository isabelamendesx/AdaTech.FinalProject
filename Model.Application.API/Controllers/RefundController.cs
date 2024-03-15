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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RefundController : ControllerBase
    {
        private readonly IRefundService _service;

        public RefundController(IRefundService service)
        {
            _service = service;
        }

        
        [HttpPost]       
        public async Task<IActionResult> CreateRefund([FromBody] RefundRequestDto request, CancellationToken ct)
        [Idempotent(ExpiresInMilliseconds = 10000)]       
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

            var createdRefund = await _service.CreateRefund(refund, ct);
            Log.Information("New Refund Submitted and {@Status} by rule with ID {@RuleId}", createdRefund.Status, createdRefund.Operations.First().ApprovalRule.Id);

            return Ok(createdRefund);
        }

        [HttpGet]
        [Route("{status}")]
        public async Task<ActionResult<IEnumerable<Refund?>>> GetAllByStatus([ValidateStatus] string status,
                                                    [FromQuery] PaginationParametersDTO paginationParameters, CancellationToken ct)
        {
            var parsedStatus = EnumParser.ParseStatus(status);

            var refunds = await _service.GetAllByStatus(parsedStatus, ct);

            if (paginationParameters.PageNumber == 0 || paginationParameters.PageSize == 0)
                return Ok(refunds);

            var paginatedRefunds = PaginationGenerator.GetPaginatedResponse(paginationParameters, refunds);

            return Ok(paginatedRefunds);
        }

        [HttpPost]
        [Route("/approve/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> ApproveRefund([FromRoute] uint id, [FromRoute] uint userId, CancellationToken ct)
        {
            var refund = await _service.ApproveRefund(id, userId, ct);
            return Ok(refund);
        }

        [HttpPost]
        [Route("/reject/{id}/{userId}")]
        [Authorize(Roles = Roles.Manager + "," + Roles.Supervisor)]
        public async Task<IActionResult> RejectRefund([FromRoute] uint id, [FromRoute] uint userId, CancellationToken ct)
        {
            var refund = await _service.RejectRefund(id, userId, ct);
            return Ok(refund);
        }

    }
}

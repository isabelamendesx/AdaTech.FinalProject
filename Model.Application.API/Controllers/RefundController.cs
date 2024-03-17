using Microsoft.AspNetCore.Mvc;
using Model.Service.Services;
using Model.Domain.Entities;
using Model.Application.API.Util;
using Model.Application.DTO.Validators;
using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Identity.Constants;
using Model.Domain.Interfaces;
using Microsoft.AspNetCore.Http.Timeouts;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.Application.API.DTO.Request;
using Model.Application.API.DTO.Response;
using Model.Application.API.Extensions;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RefundController : BaseController
    {
        private readonly IRefundService _service;
        private readonly ILogger<RefundController> _logger;
        
        public RefundController(IRefundService service, ILogger<RefundController> logger)
        {
            _service = service;
            _logger = logger;   
        }

        
        [HttpPost]
        //[Idempotent(ExpiresInMilliseconds = 10000)]
        public async Task<IActionResult> CreateRefund(/*[FromHeader] string IdempotencyKey*/ [FromBody] RefundRequestDto request, CancellationToken ct)            
        {
            ValidateWithDataAnotation();

            var refund = new Refund()
            {
                Description = request.Description,
                Category = new Category { Id = request.CategoryId },
                Status = EnumParser.ParseStatus(request.Status),
                Total = request.Total,
                OwnerID = GetUserId()
            };

            var createdRefund = await _service.CreateRefund(refund, ct);
            _logger.LogInformation("New Refund Submitted and {@Status} by rule with ID {@RuleId}", createdRefund.Status, createdRefund.Operations.First().ApprovalRule.Id);

            return Ok(createdRefund.ToResponse());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id, CancellationToken ct)
        {
            var refund = await _service.GetById(id, ct);
            return Ok(refund!.ToDetailResponse());
        }

        [HttpGet]
        [Route("/status/{status}")]
        public async Task<ActionResult<IEnumerable<Refund?>>> GetAllByStatus([ValidateStatus] string status,
                                                    [FromQuery] PaginationParametersDTO paginationParameters, CancellationToken ct)
        {
            var parsedStatus = EnumParser.ParseStatus(status);

            if (paginationParameters.PageNumber == 0 || paginationParameters.PageSize == 0)
                return Ok(await _service.GetAllByStatus(parsedStatus, ct));

            var skip = paginationParameters.PageSize * (paginationParameters.PageNumber - 1);

            var paginatedCategories = await _service.GetAllByStatusPaginated(
                    parsedStatus,
                    ct,
                    skip,
                    paginationParameters.PageSize
                );

            var response = PaginationResponseGenerator.GetPaginatedResponse
                                    (paginatedCategories, paginationParameters);

            return Ok(response);
        }

        [HttpPost]
        [Route("/approve/{id}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> ApproveRefund([FromRoute] uint id, CancellationToken ct)
        {
            var userId = GetUserId();

            var refund = await _service.ApproveRefund(id, userId, ct);

            return Ok(refund.ToDetailResponse());
        }

        [HttpPost]
        [Route("/reject/{id}")]
        [Authorize(Roles = Roles.Manager + "," + Roles.Supervisor)]
        public async Task<IActionResult> RejectRefund([FromRoute] uint id, CancellationToken ct)
        {
            var userId = GetUserId();

            var refund = await _service.RejectRefund(id, userId, ct);

            return Ok(refund.ToDetailResponse());
        }

    }
}

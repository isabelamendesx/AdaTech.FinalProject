using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Application.API.DTO.Request;
using Model.Application.API.DTO.Response;
using Model.Application.API.Extensions;
using Model.Application.API.Util;
using Model.Domain.Common;
using ICategoryService = Model.Domain.Interfaces.ICategoryService;
using IRuleService = Model.Domain.Interfaces.IRuleService;
using Rule = Model.Domain.Entities.Rule;


namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = Policies.BusinessHour)]
    public class RuleController : BaseController
    {
        private readonly IRuleService _service;
        private readonly ILogger<RuleController> _logger;

        public RuleController(IRuleService service, ILogger<RuleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id, CancellationToken ct)
        {
            var rule = await _service.GetById(id, ct);
            return Ok(rule!.ToResponse());
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParametersDTO paginationParameters, CancellationToken ct)
        {

            if (paginationParameters.PageNumber == 0 || paginationParameters.PageSize == 0)
                return Ok(await _service.GetAll(ct));

            var skip = paginationParameters.PageSize * (paginationParameters.PageNumber - 1);

            var paginatedRules = await _service.GetAllPaginated(
                    ct,
                    skip,
                    paginationParameters.PageSize
                );

            var rulesToResponse = new List<RuleResponseDTO>();

            for ( var i = 0; i < paginatedRules.Items.Count(); i++)
                rulesToResponse.Add(paginatedRules.Items.ElementAt(i).ToResponse());


            var paginatedResult = new PaginatedResult<RuleResponseDTO>() { 
                TotalCount = paginatedRules.TotalCount, 
                Items = rulesToResponse 
            };


            var response = PaginationResponseGenerator.GetPaginatedResponse<RuleResponseDTO>
                                    (paginatedResult, paginationParameters);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> CreateRule([FromBody] RuleRequestDTO request, CancellationToken ct)
        {
            ValidateWithDataAnotation();

            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            var maxValue = request.MaxValue == 0 ? decimal.MaxValue : request.MaxValue;

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = maxValue,
                Action = action,
                Category = new Domain.Entities.Category() { Id = request.CategoryId },
                IsActive = true
            };

            var createdRule = await _service.CreateRule(rule, ct);
            _logger.LogInformation("New Rule for Category {@Category} created", createdRule.Category);

            return Ok(createdRule.ToResponse());
        }

        
        [HttpPost]
        [Authorize(Roles = Roles.Manager)]
        [Route("deactivate/{ruleId}")]
        public async Task<IActionResult> DeactivateRule([FromRoute] uint ruleId, CancellationToken ct)
        {
            var deactivate = await _service.DeactivateRule(ruleId, ct);

            if (deactivate)
            {
                _logger.LogWarning("Rule with id {@Ruleid} was deactived", ruleId);        
                var response = new DeactivateRuleResponseDTO();

                response.DeactivatedRulesId.Add(ruleId);

                return Ok(response);
            }

            return BadRequest();
        }



        [HttpPost]
        [Authorize(Roles = Roles.Manager)]
        [Route("deactivate/category/{categoryId}")]
        public async Task<IActionResult> DeactivateACategorysRules([FromRoute] uint categoryId, CancellationToken ct)
        {
            var rulesIds = await _service.GetACategorysActiveRulesId(categoryId, ct);

            var deactivate = await _service.DeactivateACategorysRules(categoryId, ct);

            if (deactivate)
            {
                _logger.LogWarning("Rules for Category with id {@CategoryId} were deactived", categoryId);

                var response = new DeactivateRuleResponseDTO();

                response.DeactivatedRulesId.AddRange(rulesIds);

                return Ok(response);
            }

            return BadRequest();
        }
    }
}

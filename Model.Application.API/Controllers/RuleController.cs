using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rule = Model.Domain.Entities.Rule;
using ICategoryService = Model.Domain.Interfaces.ICategoryService;
using IRuleService = Model.Domain.Interfaces.IRuleService;
using Model.Application.API.Util;
using Model.Application.API.DTO.Request;
using Model.Application.API.DTO.Response;
using Model.Application.API.Extensions;


namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    //[Authorize(Policy = Policies.BusinessHour)]
    public class RuleController : BaseController
    {
        private readonly IRuleService _service;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<RuleController> _logger;

        public RuleController(IRuleService service, ICategoryService categoryService, ILogger<RuleController> logger)
        {
            _service = service;
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        [Route("rule/{id}")]
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

            var paginatedCategories = await _service.GetAllPaginated(
                    ct,
                    skip,
                    paginationParameters.PageSize
                );

            var response = PaginationResponseGenerator.GetPaginatedResponse
                                    (paginatedCategories, paginationParameters);

            return Ok(response);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] RuleRequestDTO request, CancellationToken ct)
        {
            ValidateWithDataAnotation();

            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            var maxValue = request.MaxValue == 0 ? decimal.MaxValue : request.MaxValue;

            var category = await _categoryService.GetById(request.CategoryId, ct);

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = maxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            var createdRule = await _service.CreateRule(rule, ct);
            _logger.LogInformation("New Rule for Category {@Category} created", createdRule.Category);

            return Ok(createdRule.ToResponse());
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/{ruleId}")]
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
       


        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/category/{categoryId}")]
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

using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Model.Application.API.Attributes;
using Model.Application.API.DTO;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Services;
using System.Data;
using Rule = Model.Domain.Entities.Rule;
using ICategoryService = Model.Domain.Interfaces.ICategoryService;
using IRuleService = Model.Domain.Interfaces.IRuleService;
using Serilog;


namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    //[Authorize(Policy = Policies.BusinessHour)]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService _service;
        private readonly ICategoryService _categoryService;

        public RuleController(IRuleService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("rule/{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id)
        {
            var rule = await _service.GetById(id, HttpContext.RequestAborted);
            return Ok(rule);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rules = await _service.GetAll(HttpContext.RequestAborted);
            return Ok(rules);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] RuleRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state: {@ModelState}", ModelState.Values);
                return UnprocessableEntity(ModelState);
            }

            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            var category = await _categoryService.GetById(request.CategoryId, HttpContext.RequestAborted);

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            var createdRule = await _service.CreateRule(rule, HttpContext.RequestAborted);
            Log.Information("New Rule for Category {@Category} created", createdRule.Category);

            return Ok(createdRule);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/{ruleId}")]
        public async Task<IActionResult> DeactivateRule([FromRoute] uint ruleId)
        {
            var deactivate = await _service.DeactivateRule(ruleId, HttpContext.RequestAborted);

            if (deactivate)
            {
                Log.Warning("Rule with id {@Ruleid} was deactived", ruleId);
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/category/{categoryId}")]
        public async Task<IActionResult> DeactivateACategorysRules([FromRoute] uint categoryId)
        {
            var deactivate = await _service.DeactivateACategorysRules(categoryId, HttpContext.RequestAborted);

            if (deactivate)
            {
                Log.Warning("Rules for Category with id {@CategoryId} were deactived", categoryId);
                return Ok();
            }

            return BadRequest();
        }
    }
}

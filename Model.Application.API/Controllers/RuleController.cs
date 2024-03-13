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
        [ClaimsAuthorize(ClaimTypes.Rule, "Create")]
        [HttpPost]
        public async Task<IActionResult> CreateRule(RuleRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            var category = await _categoryService.GetById(request.CategoryId, HttpContext.RequestAborted);

            if (category is null) 
                return BadRequest("Category not found");

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            var createdRule = await _service.CreateRule(rule, HttpContext.RequestAborted);

            return Ok(createdRule);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/{id}")]
        public async Task<IActionResult> DeactivateRule([FromRoute] uint Id)
        {
            var deactivate = await _service.DeactivateRule(Id, HttpContext.RequestAborted);
            if (deactivate)
                return Ok();
            return BadRequest();
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/category/{categoryId}")]
        public async Task<IActionResult> DeactivateACategorysRules([FromRoute] uint categoryId)
        {
            var deactivate = await _service.DeactivateACategorysRules(categoryId, HttpContext.RequestAborted);
            if (deactivate)
                return Ok();
            return BadRequest();
        }
    }
}

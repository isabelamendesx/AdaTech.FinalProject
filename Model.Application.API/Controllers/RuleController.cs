using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Application.API.Attributes;
using Model.Application.API.DTO;
using Model.Domain.Entities;
using Model.Service.Services;

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
            return Ok(await _service.GetById(id));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [Authorize(Roles = Roles.Manager)]
        [ClaimsAuthorize(ClaimTypes.Rule, "Create")]
        [HttpPost]
        public async Task<Rule> CreateRule(RuleRequestDTO request)
        {
            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            var category = await _categoryService.GetById(request.CategoryId);

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            return await _service.CreateRule(rule);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/{id}")]
        public async Task<IActionResult> DeactivateRule([FromRoute] uint Id)
        {
            var deactivate = await _service.DeactivateRule(Id);
            if (deactivate)
                return Ok();
            return BadRequest();
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        [Route("/deactivate/category/{categoryId}")]
        public async Task<IActionResult> DeactivateACategorysRules([FromRoute] uint categoryId)
        {
            var deactivate = await _service.DeactivateACategorysRules(categoryId);
            if (deactivate)
                return Ok();
            return BadRequest();
        }
    }
}

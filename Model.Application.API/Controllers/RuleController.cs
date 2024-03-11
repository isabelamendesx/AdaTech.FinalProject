using Microsoft.AspNetCore.Mvc;
using Model.Application.API.DTO;
using Model.Domain.Entities;
using Model.Service.Services;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService _service;

        public RuleController(IRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/{id}")]
        public IActionResult GetById([FromRoute] uint id)
        {
            return Ok(_service.GetById(id));
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public async Task<Rule> CreateRule(RuleRequestDTO request)
        {
            var action = request.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase) ? true : false;

            //var category = _categoryService.GetCategoryById(request.CategoryId);

            Rule rule = new Rule()
            {
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Action = action,
                //Category = category,
                IsActive = true
            };

            return await _service.CreateRule(rule);
        }

        [HttpPost]
        [Route("/deactivate/{id}")]
        public async Task<IActionResult> DeactivateRule([FromRoute] uint Id)
        {
            var deactivate = await _service.DeactivateRule(Id);
            if (deactivate)
                return Ok();
            return BadRequest();
        }
        
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

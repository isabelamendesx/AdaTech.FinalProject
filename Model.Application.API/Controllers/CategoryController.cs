using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Model.Application.API.Attributes;
using Model.Application.API.DTO;
using Model.Application.API.Util;
using Model.Application.DTO;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Services;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
   // [Authorize(Policy = Policies.BusinessHour)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category()
            {
                Name = request.Name
            };

            var createdCategory = await _service.CreateCategory(category, HttpContext.RequestAborted);

            return Ok(createdCategory);
        }

        [HttpGet]
        [Route("/category/{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id)
        {
            var category = await _service.GetById(id, HttpContext.RequestAborted);      
            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAll(HttpContext.RequestAborted);           
            return Ok(categories);
        }

    }
}

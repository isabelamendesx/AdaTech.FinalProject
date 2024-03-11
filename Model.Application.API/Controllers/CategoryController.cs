using Microsoft.AspNetCore.Mvc;
using Model.Application.API.DTO;
using Model.Application.API.Util;
using Model.Application.DTO;
using Model.Domain.Entities;
using Model.Service.Services;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO request)
        {
            var category = new Category()
            {
                Name = request.Name,
            };

            var createdCategory = await _service.CreateCategory(category);

            return Ok(createdCategory);
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_service.GetAll());
        }

    }
}

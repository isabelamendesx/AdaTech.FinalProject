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
using ICategoryService = Model.Domain.Interfaces.ICategoryService;

using Model.Service.Services;
using Serilog;

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
            {
                Log.Warning("Invalid Category model state: {@ModelState}", ModelState.Values);
                return UnprocessableEntity(ModelState);
            }

            var category = new Category()
            {
                Name = request.Name
            };

            var createdCategory = await _service.CreateCategory(category, HttpContext.RequestAborted);

            Log.Information("New category created: {@Category}", createdCategory);

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
        public async Task<IActionResult> GetAll([FromQuery] PaginationParametersDTO paginationParameters)
        {
            var categories = await _service.GetAll(HttpContext.RequestAborted);

            if (paginationParameters.PageNumber == 0 || paginationParameters.PageSize == 0)
                return Ok(categories);


            var paginatedCategories = PaginationGenerator.GetPaginatedResponse(paginationParameters, categories);

            return Ok(paginatedCategories);
        }

    }
}

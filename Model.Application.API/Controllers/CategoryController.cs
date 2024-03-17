using Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Model.Application.API.Attributes;
using Model.Application.API.Util;
using Model.Application.DTO;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using ICategoryService = Model.Domain.Interfaces.ICategoryService;

using Model.Service.Services;
using Serilog;
using Model.Service.Exceptions;
using Model.Application.API.DTO.Request;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = Policies.BusinessHour)]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO request, CancellationToken ct)
        {
            ValidateWithDataAnotation();

            var category = new Category()
            {
                Name = request.Name
            };

            var createdCategory = await _service.CreateCategory(category, ct);

            _logger.LogInformation("New category created: {@Category}", createdCategory);

            return Ok(createdCategory);
        }

        [HttpGet]
        [Route("/category/{id}")]
        public async Task<IActionResult> GetById([FromRoute] uint id, CancellationToken ct)
        {
            var category = await _service.GetById(id, ct);      
            return Ok(category);
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

            var response = PaginationResponseGenerator.GetPaginatedResponse(paginatedCategories, paginationParameters);

            return Ok(response);
        }

    }
}

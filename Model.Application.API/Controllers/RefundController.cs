using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Service.Services;
using Model.Domain.DTO;
using Model.Domain.Entities;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundService _service;

        public RefundController(IRefundService service)
        {
            _service = service;
        }

        [HttpPost]
        public Task<IActionResult> CreateRefund([FromBody] RefundRequestDto request)
        {

        }

        [HttpGet]
        [Route("{status}")]
        public Task<ActionResult<IEnumerable<Refund>>> GetAllByStatus(string status)
        {

        }

        [HttpPost]
        [Route("/approve/{id}")]
        public Task<IActionResult> ApproveRefund(uint id)
        {

        }

        [HttpPost]
        [Route("/refuse/{id}")]
        public Task<IActionResult> RefuseRefund(uint id)
        {

        }



    }
}

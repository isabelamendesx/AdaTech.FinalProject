﻿using Microsoft.AspNetCore.Mvc;
using Model.Service.Services;
using Model.Application.DTO;
using Model.Domain.Entities;
using Model.Application.API.Util;
using Model.Application.DTO.Validators;

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
        public async Task<IActionResult> CreateRefund([FromBody] RefundRequestDto request)
        {
            var refund = new Refund()
            {
                Description = request.Description,
                Category = EnumParser.ParseCategory(request.Category),
                Status = EnumParser.ParseStatus(request.Status),
                Total = request.Total
            };

            var createdRefund = await _service.CreateRefund(refund);

            return Ok(createdRefund);
        }

        [HttpGet]
        [Route("{status}")]
        public async Task<ActionResult<IEnumerable<Refund?>>> GetAllByStatus([ValidateStatus] string status)
        {
            var parsedStatus = EnumParser.ParseStatus(status);

            return Ok(await _service.GetAllByStatus(parsedStatus));
        }

        [HttpPost]
        [Route("/approve/{id}")]
        public async Task<IActionResult> ApproveRefund([FromRoute] int id)
        {
            var refund = await _service.ApproveRefund(id);
            return Ok(refund);
        }

        [HttpPost]
        [Route("/refuse/{id}")]
        public async Task<IActionResult> RefuseRefund([FromRoute] int id)
        {
            var refund = await _service.RefuseRefund(id);
            return Ok(refund);
        }

    }
}

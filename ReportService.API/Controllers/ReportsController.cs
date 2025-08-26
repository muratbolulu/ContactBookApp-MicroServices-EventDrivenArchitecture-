using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportService.API.Models;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Application.Features.Reports.Queries;

namespace ReportService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestReport([FromBody] CreateReportRequest request)
        {
            var result = await _mediator.Send(new CreateReportCommand(request.Location));
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateReport([FromBody] CreateReportCommand command)
        //{
        //    var reportId = await _mediator.Send(command);
        //    return CreatedAtAction(nameof(GetReportById), new { id = reportId }, null);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetReportById(Guid id)
        //{
        //    var result = await _mediator.Send(new GetReportByIdQuery(id));
        //    if (result == null)
        //        return NotFound();

        //    return Ok(result);
        //}
    }

}

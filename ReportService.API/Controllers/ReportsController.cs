using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Application.Features.Reports.Queries;

namespace ReportService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportCommand command)
        {
            var reportId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReportById), new { id = reportId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var result = await _mediator.Send(new GetReportByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }

}

using ContactService.Application.Features.Persons.Commands;
using ContactService.Application.Features.Persons.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : BaseController
{
    public PersonsController(IMediator mediator, IServiceProvider serviceProvider)
        : base(mediator, serviceProvider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommand command)
    {
        var validator = GetValidator<CreatePersonCommand>();
        var result = await validator.ValidateAsync(command);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetPerson), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var result = await Mediator.Send(new GetPersonByIdQuery(id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var result = await Mediator.Send(new GetAllPersonsQuery());
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var command = new DeletePersonCommand(id);

        var validator = GetValidator<DeletePersonCommand>();
        var result = await validator.ValidateAsync(command);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            }));


        var success = await Mediator.Send(new DeletePersonCommand(id));
        if (!success)
            return NotFound();

        return NoContent();
    }

}

using ContactService.Application.Features.Persons.Commands;
using ContactService.Application.Features.Persons.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreatePersonCommand> _validator;

    public PersonsController(IMediator mediator, IValidator<CreatePersonCommand> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPerson), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var result = await _mediator.Send(new GetPersonByIdQuery(id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var result = await _mediator.Send(new GetAllPersonsQuery());
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var result = await _mediator.Send(new DeletePersonCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }

}

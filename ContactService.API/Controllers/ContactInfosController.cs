using ContactService.Application.Features.ContactInfo.Commands;
using ContactService.Application.Features.ContactInfos.Commands;
using ContactService.Application.Features.ContactInfos.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactInfosController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateContactInfoCommand> _validator;

    public ContactInfosController(IValidator<CreateContactInfoCommand> validator, IMediator mediator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> AddContactInfo([FromBody] CreateContactInfoCommand command)
    {
        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new 
            { 
                Field= e.PropertyName,
                Message=e.ErrorMessage 
            }));

        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetContactInfoByIdQuery(id);
        var contactInfo = await _mediator.Send(query);

        if (contactInfo == null)
            return NotFound();
        
        return Ok(contactInfo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactInfo(Guid id)
    {
        var result = await _mediator.Send(new DeleteContactInfoCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}

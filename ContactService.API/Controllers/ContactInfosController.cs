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
    public ContactInfosController(IMediator mediator, IServiceProvider serviceProvider)
        : base(mediator, serviceProvider)
    {
    }

    [HttpPost]
    public async Task<IActionResult> AddContactInfo([FromBody] CreateContactInfoCommand command)
    {
        var validator = GetValidator<CreateContactInfoCommand>();
        var result = await validator.ValidateAsync(command);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new 
            { 
                Field= e.PropertyName,
                Message=e.ErrorMessage 
            }));

        var id = await Mediator.Send(command);
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetContactInfoByIdQuery(id);
        var contactInfo = await Mediator.Send(query);

        if (contactInfo == null)
            return NotFound();
        
        return Ok(contactInfo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactInfo(Guid id)
    {
        var result = await Mediator.Send(new DeleteContactInfoCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}

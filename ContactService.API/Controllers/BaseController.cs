using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    private readonly IServiceProvider _serviceProvider;

    protected BaseController(IMediator mediator, IServiceProvider serviceProvider)
    {
        Mediator = mediator;
        _serviceProvider = serviceProvider;
    }

    protected IValidator<T> GetValidator<T>()
    {
        return _serviceProvider.GetRequiredService<IValidator<T>>();
    }
}

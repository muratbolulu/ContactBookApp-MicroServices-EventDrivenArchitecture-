using ContactService.Application.Features.Persons.Commands;
using FluentValidation;

namespace ContactService.Application.Features.Persons.Validators;

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("PersonId boş olamaz.");
    }
}

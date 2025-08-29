using ContactService.Application.Features.Persons.Commands;
using FluentValidation;

namespace ContactService.Application.Validators;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz")
            .MaximumLength(50);

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Firma adı boş olamaz")
            .MaximumLength(100);
    }
}

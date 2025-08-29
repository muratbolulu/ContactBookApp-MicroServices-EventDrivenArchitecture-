using ContactService.Application.Features.ContactInfo.Commands;
using FluentValidation;

namespace ContactService.Application.Features.ContactInfos.Validators;

public class CreateContactInfoCommandValidator : AbstractValidator<CreateContactInfoCommand>
{
    public CreateContactInfoCommandValidator()
    {
        RuleFor(x => x.PersonId).NotEmpty();
        RuleFor(x => x.InfoType).NotEmpty().WithMessage("Bilgi türü boş olamaz.");
        RuleFor(x => x.Value).NotEmpty().WithMessage("Bilgi içeriği boş olamaz.");
    }
}

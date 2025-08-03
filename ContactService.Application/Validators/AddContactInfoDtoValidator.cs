using ContactService.Application.DTOs;
using FluentValidation;

namespace ContactService.Application.Validators;

public class AddContactInfoDtoValidator : AbstractValidator<AddContactInfoDto>
{
    public AddContactInfoDtoValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Kişi ID boş olamaz");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Geçersiz iletişim türü");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("İçerik boş olamaz")
            .MaximumLength(200);
    }
}

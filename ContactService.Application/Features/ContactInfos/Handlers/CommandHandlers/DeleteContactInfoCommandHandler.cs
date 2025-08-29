using ContactService.Application.Features.ContactInfos.Commands;
using ContactService.Application.Interfaces;
using MediatR;
using ContactInfosLocal = ContactService.Domain.Entities.ContactInfo; //neden görmediğini kontrol edeceğim.

namespace ContactService.Application.Features.ContactInfos.Handlers.CommandHandlers
{
    public class DeleteContactInfoCommandHandler : IRequestHandler<DeleteContactInfoCommand, bool>
    {
        private readonly IGenericRepository<ContactInfosLocal> _contactInfoRepository;

        public DeleteContactInfoCommandHandler(IGenericRepository<ContactInfosLocal> contactInfoRepository)
        {
            _contactInfoRepository = contactInfoRepository;
        }

        public async Task<bool> Handle(DeleteContactInfoCommand request, CancellationToken cancellationToken)
        {
            var contactInfo = await _contactInfoRepository.GetByIdAsync(request.Id);
            if (contactInfo == null)
                return false;

            await _contactInfoRepository.DeleteAsync(contactInfo);
            await _contactInfoRepository.SaveChangesAsync();

            return true;
        }
    }
}

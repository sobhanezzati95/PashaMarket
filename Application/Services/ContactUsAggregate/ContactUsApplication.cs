using Application.Dtos.ContactUsAggregate;
using Application.Interfaces.ContactUsAggregate;
using Domain;
using Domain.Entities.ContactUsAggregate;
using Framework.Application;
using Microsoft.Extensions.Logging;

namespace Application.Services.ContactUsAggregate
{
    public class ContactUsApplication : IContactUsApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContactUsApplication> _logger;
        public ContactUsApplication(IUnitOfWork unitOfWork, ILogger<ContactUsApplication> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateContactUs command)
        {
            try
            {
                var customerDiscount = ContactUs.Create(command.FullName, command.PhoneNumber, command.Email, command.Title, command.Description);
                await _unitOfWork.ContactUsRepository.Add(customerDiscount);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#ContactUsApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }
    }
}
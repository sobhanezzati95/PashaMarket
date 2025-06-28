using Application.Dtos.ContactUsAggregate;
using Application.Interfaces.ContactUsAggregate;
using Domain.Contracts;
using Domain.Entities.ContactUsAggregate;
using Framework.Application;
using Microsoft.Extensions.Logging;

namespace Application.Services.ContactUsAggregate;
public class ContactUsApplication(IUnitOfWork unitOfWork, ILogger<ContactUsApplication> logger)
    : IContactUsApplication
{
    public async Task<OperationResult> Create(CreateContactUs command, CancellationToken cancellationToken = default)
    {
        try
        {
            var customerDiscount = ContactUs.Create(command.FullName, command.PhoneNumber, command.Email, command.Title, command.Description);
            await unitOfWork.ContactUsRepository.Add(customerDiscount, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
                    "#ContactUsApplication.Create.CatchException() >> Exception: " + e.Message +
                    (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            return OperationResult.Failed(e.Message);
        }
    }
}
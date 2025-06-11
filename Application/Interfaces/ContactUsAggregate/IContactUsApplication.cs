using Application.Dtos.ContactUsAggregate;
using Framework.Application;

namespace Application.Interfaces.ContactUsAggregate
{
    public interface IContactUsApplication
    {
        Task<OperationResult> Create(CreateContactUs command);
    }
}
using Application.Dtos.UserAggregate.Role;
using Framework.Application;

namespace Application.Interfaces.UserAggregate
{
    public interface IRoleApplication
    {
        Task<OperationResult> Create(CreateRole command);
        Task<OperationResult> Edit(EditRole command);
        Task<List<RoleViewModel>> List();
        Task<EditRole> GetDetails(long id);
    }
}

using Application.Dtos.UserAggregate.Role;
using Framework.Application;

namespace Application.Interfaces.UserAggregate;
public interface IRoleApplication
{
    Task<OperationResult> Create(CreateRole command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditRole command, CancellationToken cancellationToken = default);
    Task<List<RoleViewModel>> List(CancellationToken cancellationToken = default);
    Task<EditRole> GetDetails(long id, CancellationToken cancellationToken = default);
}
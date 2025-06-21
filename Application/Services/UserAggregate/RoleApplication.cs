using Application.Dtos.UserAggregate.Role;
using Application.Services.ProductAggregate;
using Domain.Contracts;
using Domain.Entities.UserAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Interfaces.UserAggregate;
public class RoleApplication(IUnitOfWork unitOfWork, ILogger<SlideApplication> logger)
    : IRoleApplication
{
    public async Task<OperationResult> Create(CreateRole command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await unitOfWork.RoleRepository.Exists(x => x.Name == command.Name))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var role = Role.Create(command.Name);
            await unitOfWork.RoleRepository.Add(role, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#RoleApplication.Create.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Edit(EditRole command, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await unitOfWork.RoleRepository.GetById(command.Id, cancellationToken);
            if (role == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            if (await unitOfWork.RoleRepository.Exists(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            await unitOfWork.RoleRepository.Update(role);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#RoleApplication.Edit.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<EditRole> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await unitOfWork.RoleRepository.GetById(id, cancellationToken);
            return new EditRole
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#RoleApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<RoleViewModel>> List(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await unitOfWork.RoleRepository.GetAllAsQueryable(cancellationToken);
            return await roles
                .Select(x => new RoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#RoleApplication.List.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}
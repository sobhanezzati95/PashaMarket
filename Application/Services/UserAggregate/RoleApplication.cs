using Application.Dtos.UserAggregate.Role;
using Application.Services.ProductAggregate;
using Domain;
using Domain.Entities.UserAggregate;
using Framework.Application;
using Microsoft.Extensions.Logging;

namespace Application.Interfaces.UserAggregate
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SlideApplication> _logger;

        public RoleApplication(IUnitOfWork unitOfWork, ILogger<SlideApplication> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateRole command)
        {
            try
            {
                if (await _unitOfWork.RoleRepository.Exists(x => x.Name == command.Name))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var role = Role.Create(command.Name);

                await _unitOfWork.RoleRepository.Add(role);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#RoleApplication.Create.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Edit(EditRole command)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetById(command.Id);
                if (role == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.RoleRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);


                await _unitOfWork.RoleRepository.Update(role);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#RoleApplication.Edit.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<EditRole> GetDetails(long id)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetById(id);
                return new EditRole
                {
                    Id = role.Id,
                    Name = role.Name,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#RoleApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<RoleViewModel>> List()
        {
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetAllAsQueryable();
                return roles.Select(x => new RoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#RoleApplication.List.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}

using Application.Dtos.UserAggregate.User;
using Application.Services.ProductAggregate;
using Domain;
using Domain.Entities.UserAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Interfaces.UserAggregate;
public class UserApplication(IUnitOfWork unitOfWork,
                             ILogger<SlideApplication> logger,
                             IFileUploader fileUploader,
                             IAuthenticationHelper authenticationHelper,
                             IPasswordHasher passwordHasher)
    : IUserApplication
{
    public async Task<OperationResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetById(command.Id, cancellationToken);
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return OperationResult.Failed(ApplicationMessages.PasswordsNotMatch);

            var password = passwordHasher.Hash(command.Password);
            user.ChangePassword(password);
            await unitOfWork.UserRepository.Update(user, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Edit(EditUser command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetById(command.Id, cancellationToken);
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            if (await unitOfWork.UserRepository.Exists(x =>
                (x.Username == command.Username || x.Email == command.Email) && x.Id != command.Id, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            user.Edit(command.Fullname, command.Username, command.Mobile, command.Email, command.BirthDate);
            await unitOfWork.UserRepository.Update(user, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.Edit.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<UserViewModel> GetUserBy(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetById(id, cancellationToken);
            return new UserViewModel()
            {
                Fullname = user.Fullname,
                Mobile = user.Email
            };
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.GetUserBy.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<UserViewModel>> GetUsers(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await unitOfWork.UserRepository.GetAllAsQueryable(cancellationToken);
            return await users
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Fullname = x.Fullname
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.GetUsers.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<EditUser> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetById(id, cancellationToken);
            return new EditUser()
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Mobile = user.Email,
                Username = user.Username,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Password = user.Password
            };
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Login(Login command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetByUsername(command.Username, cancellationToken);
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.WrongUserPass);

            var hashed = passwordHasher.Hash(command.Password);
            var result = passwordHasher.Check(user.Password, command.Password);
            if (!result.Verified)
                return OperationResult.Failed(ApplicationMessages.WrongUserPass);

            var authViewModel = new AuthenticationViewModel(user.Id, user.RoleId, user.Fullname, user.Username, user.Email);
            await authenticationHelper.Signin(authViewModel);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.Login.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task Logout()
       => await authenticationHelper.SignOut();
    public async Task<OperationResult> Register(RegisterUser command, CancellationToken cancellationToken = default)
    {
        try
        {

            if (await unitOfWork.UserRepository.Exists(x => x.Username == command.Username || x.Email == command.Email, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var password = passwordHasher.Hash(command.Password);
            var user = User.Register(command.Username, command.Email, password);
            await unitOfWork.UserRepository.Add(user, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            var authViewModel = new AuthenticationViewModel(user.Id, user.RoleId, user.Fullname, user.Username, user.Email);
            await authenticationHelper.Signin(authViewModel);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.Register.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<UserViewModel>> Search(UserSearchModel searchModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.UserRepository.GetAllWithIncludesAndThenInCludes(
            predicate: null,
            orderBy: x => x.OrderByDescending(p => p.Id),
            isTracking: false,
            ignoreQueryFilters: false,
            includeProperties: null,
            thenInclude: query => query.Include(x => x.Role));

            if (await query.AnyAsync(cancellationToken) == false)
                return [];

            if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
                query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

            if (!string.IsNullOrWhiteSpace(searchModel.Username))
                query = query.Where(x => x.Username.Contains(searchModel.Username));

            if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                query = query.Where(x => x.Email.Contains(searchModel.Mobile));

            if (searchModel.RoleId > 0)
                query = query.Where(x => x.RoleId == searchModel.RoleId);

            return await query
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Fullname = x.Fullname,
                    Mobile = x.Email,
                    Role = x.Role.Name,
                    RoleId = x.RoleId,
                    Username = x.Username,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.Search.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}
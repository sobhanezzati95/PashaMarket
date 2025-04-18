using Application.Dtos.UserAggregate.User;
using Application.Services.ProductAggregate;
using Domain;
using Domain.Entities.UserAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Interfaces.UserAggregate
{
    public class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SlideApplication> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthenticationHelper _authenticationHelper;
        public UserApplication(IUnitOfWork unitOfWork, ILogger<SlideApplication> logger, IFileUploader fileUploader, IAuthenticationHelper authenticationHelper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileUploader = fileUploader;
            _authenticationHelper = authenticationHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult> ChangePassword(ChangePassword command)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetById(command.Id);
                if (user == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (command.Password != command.RePassword)
                    return OperationResult.Failed(ApplicationMessages.PasswordsNotMatch);

                var password = _passwordHasher.Hash(command.Password);
                user.ChangePassword(password);
                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Edit(EditUser command)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetById(command.Id);
                if (user == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.UserRepository.Exists(x =>
                    (x.Username == command.Username || x.Email == command.Email) && x.Id != command.Id))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                user.Edit(command.Fullname, command.Username, command.Mobile, command.Email, command.BirthDate);
                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.Edit.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<UserViewModel> GetUserBy(long id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetById(id);
                return new UserViewModel()
                {
                    Fullname = user.Fullname,
                    Mobile = user.Email
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.GetUserBy.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsQueryable();
                return users.Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Fullname = x.Fullname
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.GetUsers.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<EditUser> GetDetails(long id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetById(id);
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
                _logger.LogError(e,
                "#UserApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Login(Login command)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByUsername(command.Username);
                if (user == null)
                    return OperationResult.Failed(ApplicationMessages.WrongUserPass);

                var hashed = _passwordHasher.Hash(command.Password);
                var result = _passwordHasher.Check(user.Password, command.Password);
                if (!result.Verified)
                    return OperationResult.Failed(ApplicationMessages.WrongUserPass);

                var authViewModel = new AuthenticationViewModel(user.Id, user.RoleId, user.Fullname, user.Username, user.Email);

                await _authenticationHelper.Signin(authViewModel);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.Login.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task Logout()
        {
            await _authenticationHelper.SignOut();
        }

        public async Task<OperationResult> Register(RegisterUser command)
        {
            try
            {

                if (await _unitOfWork.UserRepository.Exists(x => x.Username == command.Username || x.Email == command.Email))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var password = _passwordHasher.Hash(command.Password);
                var user = User.Register(command.Username, command.Email, password);

                await _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.CommitAsync();

                var authViewModel = new AuthenticationViewModel(user.Id, user.RoleId, user.Fullname, user.Username, user.Email);
                await _authenticationHelper.Signin(authViewModel);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.Register.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<UserViewModel>> Search(UserSearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.UserRepository.GetAllWithIncludesAndThenInCludes(
                predicate: null,
                orderBy: x => x.OrderByDescending(p => p.Id),
                isTracking: false,
                ignoreQueryFilters: false,
                includeProperties: null,
                thenInclude: query => query.Include(x => x.Role));

                if (query.Any() == false)
                    return new();

                if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
                    query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

                if (!string.IsNullOrWhiteSpace(searchModel.Username))
                    query = query.Where(x => x.Username.Contains(searchModel.Username));

                if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                    query = query.Where(x => x.Email.Contains(searchModel.Mobile));

                if (searchModel.RoleId > 0)
                    query = query.Where(x => x.RoleId == searchModel.RoleId);

                return query.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Fullname = x.Fullname,
                    Mobile = x.Email,
                    Role = x.Role.Name,
                    RoleId = x.RoleId,
                    Username = x.Username,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#UserApplication.Search.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}

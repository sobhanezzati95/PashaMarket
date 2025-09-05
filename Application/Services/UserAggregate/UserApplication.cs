using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Domain.Entities.UserAggregate;
using Framework.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Application.Services.UserAggregate;
public class UserApplication(IAuthenticationHelper authenticationHelper,
                             IEmailService emailService,
                             IUrlHelperFactory urlHelperFactory,
                             IHttpContextAccessor httpContextAccessor,
                             UserManager<User> userManager,
                             SignInManager<User> signInManager,
                             ILogger<UserApplication> logger)
    : IUserApplication
{
    public async Task<OperationResult> Register(RegisterUser command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = User.Register(command.Username, command.Email);
            var userRegistrationResult = await userManager.CreateAsync(user, command.Password);
            if (!userRegistrationResult.Succeeded)
                return OperationResult.Failed(userRegistrationResult.Errors.First().Description);

            var addToRoleResult = await userManager.AddToRoleAsync(user, "SystemUser");
            if (!addToRoleResult.Succeeded)
                return OperationResult.Failed(addToRoleResult.Errors.First().Description);

            var httpContext = httpContextAccessor.HttpContext;
            var urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()));
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            string callbackUrl = urlHelper.Page("/ConfirmEmail", null, new { UserId = user.Id, Token = token }, httpContext.Request.Scheme);
            string body = GetConfirmEmailBody(callbackUrl);
            await emailService.Execute(command.Email, body, ConfirmEmailTitle);
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
    public async Task<OperationResult> Edit(EditUser command, CancellationToken cancellationToken = default)
    {
        try
        {
            //if (await unitOfWork.UserRepository.Exists(x =>
            //    (x.Username == command.Username || x.Email == command.Email) && x.Id != command.Id, cancellationToken))
            //    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);
            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);
            user.Edit(command.Fullname,
                      command.Username,
                      command.Mobile,
                      command.NationalCode,
                      command.Email,
                      command.BirthDate);
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded
                 ? OperationResult.Succeeded()
                 : OperationResult.Failed(result.Errors.First().Description);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.Edit.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (command.NewPassword != command.RePassword)
                return OperationResult.Failed(ApplicationMessages.PasswordsNotMatch);

            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if (user is null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            var result = await userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (result.Succeeded)
                return OperationResult.Succeeded();
            return OperationResult.Failed(ApplicationMessages.OperationFailed);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.ChangePassword.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Login(Login command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByNameAsync(command.Username);
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.WrongUserPass);

            if (!await userManager.IsEmailConfirmedAsync(user))
                return OperationResult.Failed(ApplicationMessages.PleaseConfirmEmail);

            var result = await signInManager.PasswordSignInAsync(user, command.Password, command.RememberMe, false);
            if (result.Succeeded)
                return OperationResult.Succeeded();
            return OperationResult.Failed(ApplicationMessages.LoginFailed);
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
       => await signInManager.SignOutAsync();
    public async Task<EditUser> GetUserInfo(CancellationToken cancellationToken = default)
    {
        try
        {
            long userId = authenticationHelper.CurrentUserId();
            var user = await userManager.FindByIdAsync(userId.ToString());
            return new EditUser
            {
                BirthDate = user.BirthDate,
                Mobile = user.PhoneNumber,
                Email = user.Email,
                Fullname = user.Fullname,
                NationalCode = user.NationalCode,
                Username = user.UserName,
                Id = user.Id,
            };
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.GetUserInfo.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> ConfirmEmail(string userId, string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);
            var result = await userManager.ConfirmEmailAsync(user, token);
            return !result.Succeeded
                  ? OperationResult.Failed(result.Errors.First().Description)
                  : OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.ConfirmEmail.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> ForgetPassword(ForgetPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user is null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);
            if (!await userManager.IsEmailConfirmedAsync(user))
                return OperationResult.Failed(ApplicationMessages.PleaseConfirmEmail);

            var httpContext = httpContextAccessor.HttpContext;
            var urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()));
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = urlHelper.Page("/ResetPassword", null, new { UserId = user.Id, Token = token }, httpContext.Request.Scheme);
            string body = GetResetPasswordBody(callbackUrl);
            await emailService.Execute(command.Email, body, ResetPasswordTitle);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.FindByEmailAsync.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> ResetPassword(ResetPassword command, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByIdAsync(command.UserId.ToString());
            if (user is null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            var result = await userManager.ResetPasswordAsync(user, command.Token, command.Password);
            return result.Succeeded
                            ? OperationResult.Succeeded()
                            : OperationResult.Failed(result.Errors.First().Description);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#UserApplication.ResetPassword.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    private static string GetConfirmEmailBody(string callbackUrl)
        => $"لطفا برای فعالسازی حساب کاربری بر روی لینک زیر کلیک کنید!  <br/> <a href={callbackUrl}> Link </a>";
    private static string GetResetPasswordBody(string callbackUrl)
        => $"لطفا برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید!  <br/> <a href={callbackUrl}> Link </a>";
    private static readonly string ConfirmEmailTitle = "فعالسازی حساب کاربری پاشامارکت";
    private static readonly string ResetPasswordTitle = "فراموشی کلمه عبور پاشامارکت";
}
using DemoIdentityApp.DAL.Extensions;
using DemoIdentityApp.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DemoIdentityApp.API.Services;

public interface IUserService
{
    Task<UserManagerResponse> RegisterUserAsync(RegisterVm? model);

}

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserManagerResponse> RegisterUserAsync(RegisterVm? model)
    {
        if (model is null)
        {
            throw new NullReferenceException("Register model is null");
        }

        var identityUser = new IdentityUser()
        {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(identityUser, model.Password);

        if (result.Succeeded)
        {
            return new UserManagerResponse()
            {
                Message = "User created successfully",
                IsSuccess = true,
            };
        }


        return new UserManagerResponse()
        {
            Message = "User didn't created",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
        };
    }
}
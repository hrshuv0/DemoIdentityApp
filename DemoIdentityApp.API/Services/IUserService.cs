using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoIdentityApp.DAL.Extensions;
using DemoIdentityApp.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DemoIdentityApp.API.Services;

public interface IUserService
{
    Task<UserManagerResponse> RegisterUserAsync(RegisterVm? model);
    Task<UserManagerResponse> LoginUserAsync(LoginVm? model);

}

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
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

    public async Task<UserManagerResponse> LoginUserAsync(LoginVm? model)
    {
        var user = await _userManager.FindByEmailAsync(model!.Email);

        if (user is null)
            return new UserManagerResponse()
            {
                Message = $"There is no user with this {model.Email} Email address",
                IsSuccess = false
            };

        var result = await _userManager.CheckPasswordAsync(user, model.Password);
        
        if (!result)
            return new UserManagerResponse()
            {
                Message = "Invalid password",
                IsSuccess = false
            };

        var claims = new[]
        {
            new Claim("Email", model.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value));
        
        var token = new JwtSecurityToken(
            claims : claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserManagerResponse()
        {
            Message = tokenString,
            IsSuccess = true,
            ExpireDate = token.ValidTo
        };
    }
}
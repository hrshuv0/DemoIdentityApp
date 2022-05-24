using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoIdentityApp.API.Services;
using DemoIdentityApp.DAL;
using DemoIdentityApp.DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoIdentityApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IEmailSender _emailSender;

    public AuthController(IUserService userService, IEmailSender emailSender)
    {
        _userService = userService;
        _emailSender = emailSender;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterVm model)
    {
        if (!ModelState.IsValid) 
            return BadRequest("Some properties are not valid");

        var result = await _userService.RegisterUserAsync(model);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginVm model)
    {
        if (!ModelState.IsValid) return BadRequest("some properties are not valid");

        var result = await _userService.LoginUserAsync(model);

        if (!result.IsSuccess) return BadRequest(result);

        var subject = "new login";
        var htmlMessage = EmailHelperView.NewLogin;
            
        await _emailSender.SendEmailAsync(model.Email, subject, htmlMessage);
        
        return Ok(result);
    }
    
    

}
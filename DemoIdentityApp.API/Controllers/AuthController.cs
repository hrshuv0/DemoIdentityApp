using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoIdentityApp.API.Services;
using DemoIdentityApp.DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoIdentityApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
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
        
        return Ok(result);
    }
    
    

}
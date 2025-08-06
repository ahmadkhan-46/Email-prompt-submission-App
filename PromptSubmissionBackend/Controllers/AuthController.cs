using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromptSubmissionBackend.DTOs;
using PromptSubmissionBackend.Models;
using PromptSubmissionBackend.Services;

namespace PromptSubmissionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.SignupAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("user-login")]
    public async Task<IActionResult> UserLogin(LoginRequest request)
    {
           var result = await _authService.LoginAsync(request, "user");
        if (!result.Success)
            return Unauthorized(new { message = result.Message });

        return Ok(new
        {
            token = result.Token,
            message = result.Message
        });
    }

    [HttpPost("admin-login")]
    public async Task<AuthResult> AdminLogin(LoginRequest request)
    {
        return await _authService.LoginAsync(request, "admin");
    }

    [Authorize(Roles = "admin")]
    [HttpPost("create-admin")]
    public async Task<IActionResult> CreateAdmin([FromBody] AdminCreateDto dto)
    {
        var result = await _authService.CreateAdminAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

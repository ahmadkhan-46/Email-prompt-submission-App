using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromptSubmissionBackend.Data;
using PromptSubmissionBackend.DTOs;
using PromptSubmissionBackend.Models;
using PromptSubmissionBackend.Services;
using System.Security.Claims;

namespace PromptSubmissionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromptController(PromptService promptService) : ControllerBase
{
    private readonly PromptService _promptService = promptService;

    [HttpPost("submit")]
    [Authorize(Roles = "user")]
    
    public async Task<IActionResult> SubmitPrompt([FromBody] PromptRequestDto requestDto)
    {
        var senderEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid user ID.");

        var prompt = new PromptRequest
        {
            PromptText = requestDto.PromptText,
            RecipientEmail = requestDto.RecipientEmail,
            UserId = userId,
            Status = "Pending"
        };

        await _promptService.SavePromptPendingAsync(senderEmail!, requestDto);

        return Ok(new { message = "Prompt saved. It will be submitted shortly." });
    }


    [HttpGet("my-prompts")] 
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetMyPrompts()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid user ID.");

        var prompts = await _promptService.GetPromptsByUserIdAsync(userId);
        return Ok(prompts);
    }
}

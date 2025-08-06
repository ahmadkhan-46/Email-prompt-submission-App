using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromptSubmissionBackend.Data;

namespace PromptSubmissionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")] 
public class AdminController(ApplicationDbContext _db) : ControllerBase
{
    [HttpGet("admin-requests")]
    public async Task<IActionResult> GetAllPrompts()
    {
        var prompts = await _db.Prompts
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new
            {
                p.Id,
                p.PromptText,
                p.CreatedAt,
                UserEmail = p.User.Email
            })
            .ToListAsync();

        return Ok(prompts);
    }

    [HttpGet("request-count")]
    public async Task<IActionResult> GetRequestCount()
    {
        var count = await _db.Prompts.CountAsync();
        return Ok(new { totalRequests = count });
    }
}

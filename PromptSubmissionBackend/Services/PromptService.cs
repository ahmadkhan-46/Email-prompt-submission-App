using Microsoft.EntityFrameworkCore;
using PromptSubmissionBackend.Data;
using PromptSubmissionBackend.DTOs;
using PromptSubmissionBackend.Models;

namespace PromptSubmissionBackend.Services;

public class PromptService(ApplicationDbContext context, EmailService emailService)
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailService _emailService = emailService;

    public async Task SavePromptPendingAsync(string senderEmail, PromptRequestDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == senderEmail);
        if (user == null)
            throw new Exception("User not found.");

        var prompt = new PromptRequest
        {
            UserId = user.Id,
            PromptText = dto.PromptText,
            RecipientEmail = dto.RecipientEmail,
            Status = "Pending", 
            CreatedAt = DateTime.UtcNow
        };

        _context.Prompts.Add(prompt);
        await _context.SaveChangesAsync();
    }


    public async Task<List<PromptRequest>> GetAllAsync()
    {
        return await _context.Prompts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
    public async Task<List<PromptRequest>> GetPromptsByUserIdAsync(int userId)
    {
        return await _context.Prompts
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

}

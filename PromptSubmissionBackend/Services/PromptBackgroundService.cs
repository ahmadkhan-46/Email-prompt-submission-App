using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PromptSubmissionBackend.Data;
using PromptSubmissionBackend.Models;

namespace PromptSubmissionBackend.Services
{
    public class PromptBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PromptBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var geminiService = scope.ServiceProvider.GetRequiredService<GeminiService>();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<PromptBackgroundService>>();

                var pendingPrompts = await db.Prompts
                    .Where(p => p.Status == "Pending")
                    .ToListAsync(stoppingToken);

                logger.LogInformation("Found {Count} pending prompts to process", pendingPrompts.Count);

                foreach (var prompt in pendingPrompts)
                {
                    try
                    {
                        logger.LogInformation("Processing prompt ID {Id}", prompt.Id);

                        // Generate AI-based email text
                        var emailBody = await geminiService.GenerateEmailFromPromptAsync(prompt.PromptText);

                        if (string.IsNullOrWhiteSpace(emailBody))
                        {
                            logger.LogWarning("Gemini returned empty content for prompt ID {Id}. Using original prompt text.", prompt.Id);
                            emailBody = $"(Fallback) Original Prompt:\n\n{prompt.PromptText}";
                        }

                        try
                        {
                            await emailService.SendEmailAsync(prompt.RecipientEmail, "AI Generated Email", emailBody);
                            prompt.Status = "Submitted";
                            logger.LogInformation("Prompt ID {Id} processed and status updated to Submitted", prompt.Id);
                        }
                        catch (Exception emailEx)
                        {
                            logger.LogError(emailEx, "Failed to send email for prompt ID {Id}. Keeping status as Pending.", prompt.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing prompt ID {Id}. Will retry in next cycle.", prompt.Id);
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}

namespace PromptSubmissionBackend.Models;

public class SignupRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
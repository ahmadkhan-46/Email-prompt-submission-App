namespace PromptSubmissionBackend.Services;

public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }

    public AuthResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    public AuthResult() { }

    public AuthResult(bool success, string message, string token)
    {
        Success = success;
        Message = message;
        Token = token;
    }
}
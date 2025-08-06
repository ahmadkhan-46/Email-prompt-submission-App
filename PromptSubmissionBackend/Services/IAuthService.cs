using PromptSubmissionBackend.DTOs;
using PromptSubmissionBackend.Models;

namespace PromptSubmissionBackend.Services;

public interface IAuthService
{
    Task<AuthResult> SignupAsync(SignupRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request, string role);
    Task<AuthResult> CreateAdminAsync(AdminCreateDto dto);

}

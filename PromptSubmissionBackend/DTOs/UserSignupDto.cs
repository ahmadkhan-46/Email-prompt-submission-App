using System.ComponentModel.DataAnnotations;

namespace PromptSubmissionBackend.DTOs
{
    public class UserSignupDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}

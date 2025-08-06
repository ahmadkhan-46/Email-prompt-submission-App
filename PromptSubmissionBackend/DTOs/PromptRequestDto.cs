using PromptSubmissionBackend.Models;

namespace PromptSubmissionBackend.DTOs
{
    public class PromptRequestDto
    {
        public required string RecipientEmail { get; set; }
        public required string PromptText { get; set; }
        
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PromptSubmissionBackend.Models
{
    public class PromptRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string PromptText { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public required string  RecipientEmail { get; set; }
        public string Status { get; set; } = "Pending"; 


        public required int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TaskTracker.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime? Expiration { get; set; }
    }
}

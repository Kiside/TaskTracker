using System.ComponentModel.DataAnnotations;

namespace TaskTracker.DTOs
{
    public class CreateTaskItemRequest
    {
        [Required]
        [StringLength(100)]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}

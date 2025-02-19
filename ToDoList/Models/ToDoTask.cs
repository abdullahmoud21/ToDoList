using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoTask
    {
        
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public string? Attachment { get; set; }
    }
}

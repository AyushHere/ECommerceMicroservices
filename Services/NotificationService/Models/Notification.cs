using System.ComponentModel.DataAnnotations;

namespace NotificationService.Model
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Recipient { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

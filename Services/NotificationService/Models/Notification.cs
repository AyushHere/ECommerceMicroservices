using System.ComponentModel.DataAnnotations;

namespace NotificationService.Model
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
            
        public string Recipient { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

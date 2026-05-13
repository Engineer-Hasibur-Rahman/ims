namespace ims.Domain.Entities
{
    public class Notification : BaseEntaity
    {
        public Guid? UserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}

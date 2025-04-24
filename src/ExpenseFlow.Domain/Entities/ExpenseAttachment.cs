namespace ExpenseFlow.Domain.Entities;

    public class ExpenseAttachment
    {
        public Guid Id { get; set; }
        public Guid ExpenseId { get; set; }

        public string FileUrl { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

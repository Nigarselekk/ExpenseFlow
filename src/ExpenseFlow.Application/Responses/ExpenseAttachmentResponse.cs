 

namespace ExpenseFlow.Application.Responses;

    public class ExpenseAttachmentResponse
    {
        public Guid Id { get; set; }
        public Guid ExpenseId { get; set; }
        public string FileUrl { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
    }


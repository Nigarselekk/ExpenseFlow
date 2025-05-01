

namespace ExpenseFlow.Application.Requests;

    public class ExpenseAttachmentRequest
    {
        public Guid ExpenseId { get; set; }
        public string FileUrl { get; set; } = null!;
        public string FileType { get; set; } = null!;
    }


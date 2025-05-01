
namespace ExpenseFlow.Application.Requests;

    public class ExpenseRequest
    {
        public Guid PersonnelId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
    }


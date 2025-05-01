namespace ExpenseFlow.Application.Requests;

    public class ExpenseCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }


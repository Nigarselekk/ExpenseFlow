 

namespace ExpenseFlow.Application.Responses;

    public class ExpenseCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }


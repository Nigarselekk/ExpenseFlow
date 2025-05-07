namespace ExpenseFlow.Application.Dtos.Reports;
public class ExpenseByCategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}
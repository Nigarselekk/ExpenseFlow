using MediatR;
using ExpenseFlow.Application.Dtos.Reports;
using System;

namespace ExpenseFlow.Application.Cqrs.Queries.Reports
{
    /// <summary>
    /// Query to get expenses by category within a specified date range.
    /// </summary>
    public record GetExpensesByCategoryQuery(DateTime? From = null, DateTime? To = null)
        : IRequest<List<ExpenseByCategoryDto>>;
}
using MediatR;
using ExpenseFlow.Application.Responses;


namespace ExpenseFlow.Application.Cqrs.Queries;

public record GetAllExpenseCategoriesQuery()
    : IRequest<List<ExpenseCategoryResponse>>;

public record GetExpenseCategoryByIdQuery(int Id)
    : IRequest<ExpenseCategoryResponse>;


using MediatR;
using ExpenseFlow.Application.Responses;
using System.Collections.Generic;

namespace ExpenseFlow.Application.Cqrs.Queries;

public record GetAllExpenseCategoriesQuery()
    : IRequest<List<ExpenseCategoryResponse>>;

public record GetExpenseCategoryByIdQuery(int Id)
    : IRequest<ExpenseCategoryResponse>;


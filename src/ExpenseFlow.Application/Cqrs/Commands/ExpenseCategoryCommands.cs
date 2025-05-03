using MediatR;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Application.Cqrs.Commands;

public record CreateExpenseCategoryCommand(ExpenseCategoryRequest Request)
    : IRequest<ExpenseCategoryResponse>;

public record UpdateExpenseCategoryCommand(int Id, ExpenseCategoryRequest Request)
    : IRequest<Unit>;

public record DeleteExpenseCategoryCommand(int Id)
    : IRequest<Unit>;


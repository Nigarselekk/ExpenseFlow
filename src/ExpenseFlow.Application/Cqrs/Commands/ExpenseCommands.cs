using MediatR;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Application.Cqrs.Commands;

    public record CreateExpenseCommand(ExpenseRequest Request)
        : IRequest<ExpenseResponse>;

    public record UpdateExpenseCommand(Guid Id, ExpenseRequest Request)
        : IRequest;

    public record DeleteExpenseCommand(Guid Id)
        : IRequest;


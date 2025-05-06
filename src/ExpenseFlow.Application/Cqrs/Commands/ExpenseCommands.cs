using MediatR;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Application.Cqrs.Commands;

public record CreateExpenseCommand(Guid PersonnelId, ExpenseRequest Request)
: IRequest<ExpenseResponse>;


public record UpdateExpenseCommand(Guid Id, ExpenseRequest Request)
    : IRequest<Unit>;

public record DeleteExpenseCommand(Guid Id)
    : IRequest<Unit>;

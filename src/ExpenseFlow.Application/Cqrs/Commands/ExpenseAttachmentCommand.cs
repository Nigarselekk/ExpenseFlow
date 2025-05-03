 using MediatR;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Application.Cqrs.Commands;

    public record CreateExpenseAttachmentCommand(ExpenseAttachmentRequest Request)
        : IRequest<ExpenseAttachmentResponse>;

    public record UpdateExpenseAttachmentCommand(Guid Id, ExpenseAttachmentRequest Request)
        : IRequest<Unit>;

    public record DeleteExpenseAttachmentCommand(Guid Id)
        : IRequest<Unit>;


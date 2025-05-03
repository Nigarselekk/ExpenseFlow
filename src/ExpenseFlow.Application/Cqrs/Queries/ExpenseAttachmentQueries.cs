using ExpenseFlow.Application.Responses;
using MediatR;

namespace ExpenseFlow.Application.Cqrs.Queries;

public record GetAllExpenseAttachmentsQuery() 
    : IRequest<List<ExpenseAttachmentResponse>>;
public record GetExpenseAttachmentByIdQuery(Guid Id)
    : IRequest<ExpenseAttachmentResponse>;
public record GetExpenseAttachmentsByPersonnelIdQuery(Guid PersonnelId)
    : IRequest<List<ExpenseAttachmentResponse>>;
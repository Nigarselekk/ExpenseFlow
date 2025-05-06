using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using MediatR;

namespace ExpenseFlow.Application.Cqrs.Commands;

public record CreatePaymentTransactionCommand(PaymentTransactionRequest Request)
    : IRequest<PaymentTransactionResponse>;

public record UpdatePaymentTransactionCommand(Guid Id, PaymentTransactionRequest Request)
    : IRequest<Unit>;
public record DeletePaymentTransactionCommand(Guid Id)
    : IRequest<Unit>;
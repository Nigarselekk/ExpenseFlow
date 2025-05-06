
using ExpenseFlow.Application.Responses;
using MediatR;

namespace ExpenseFlow.Application.Cqrs.Queries;
public record GetAllPaymentTransactionsQuery() : IRequest<List<PaymentTransactionResponse>>;
public record GetPaymentTransactionByIdQuery(Guid Id) : IRequest<PaymentTransactionResponse>;

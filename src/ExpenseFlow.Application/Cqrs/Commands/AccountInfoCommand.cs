using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using MediatR;

namespace ExpenseFlow.Application.Cqrs.Commands
{
    public record CreateAccountInfoCommand(AccountInfoRequest Request)
        : IRequest<AccountInfoResponse>;

    public record UpdateAccountInfoCommand(Guid Id, AccountInfoRequest Request)
        : IRequest<Unit>;   

    public record DeleteAccountInfoCommand(Guid Id)
        : IRequest<Unit>; 
}
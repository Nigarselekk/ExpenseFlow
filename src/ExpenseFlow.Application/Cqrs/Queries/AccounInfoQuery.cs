using MediatR;
using ExpenseFlow.Application.Responses;


namespace ExpenseFlow.Application.Cqrs.Queries
{
    public record GetAllAccountInfosQuery() 
        : IRequest<List<AccountInfoResponse>>;

    public record GetAccountInfoByIdQuery(Guid Id)
        : IRequest<AccountInfoResponse>;
}
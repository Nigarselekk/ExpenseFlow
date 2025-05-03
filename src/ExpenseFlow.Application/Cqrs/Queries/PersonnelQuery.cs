using MediatR;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Application.Cqrs.Queries;

    public record GetAllPersonnelQuery() : IRequest<List<PersonnelResponse>>;
    public record GetPersonnelByIdQuery(Guid Id) : IRequest<PersonnelResponse>;



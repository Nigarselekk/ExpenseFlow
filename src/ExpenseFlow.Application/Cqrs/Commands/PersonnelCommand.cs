using MediatR;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Cqrs.Commands;

    public record CreatePersonnelCommand(string ApplicationUserId, PersonnelRequest Request)
        : IRequest<PersonnelResponse>;

    public record UpdatePersonnelCommand(Guid Id, PersonnelRequest Request) : IRequest<Unit>;

    public record DeletePersonnelCommand(Guid Id) : IRequest<Unit>;

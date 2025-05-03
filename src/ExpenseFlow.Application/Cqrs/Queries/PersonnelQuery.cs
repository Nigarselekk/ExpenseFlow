using MediatR;
using ExpenseFlow.Application.Responses;
using System.Collections.Generic;

namespace ExpenseFlow.Application.Cqrs.Queries;

    public record GetAllPersonnelQuery() : IRequest<List<PersonnelResponse>>;
    public record GetPersonnelByIdQuery(Guid Id) : IRequest<PersonnelResponse>;

    

using MediatR;
using ExpenseFlow.Application.Responses;


namespace ExpenseFlow.Application.Cqrs.Queries;

    public record GetAllExpensesQuery()
        : IRequest<List<ExpenseResponse>>;

    public record GetExpenseByIdQuery(Guid Id)
        : IRequest<ExpenseResponse>;

    public record GetExpensesByPersonnelQuery(Guid PersonnelId)
        : IRequest<List<ExpenseResponse>>;

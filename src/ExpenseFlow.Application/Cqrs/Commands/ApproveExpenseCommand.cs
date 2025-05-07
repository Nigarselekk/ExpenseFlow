using MediatR;

namespace ExpenseFlow.Application.Cqrs.Commands
{
       public record ApproveExpenseCommand(Guid ExpenseId) : IRequest<Unit>;
}
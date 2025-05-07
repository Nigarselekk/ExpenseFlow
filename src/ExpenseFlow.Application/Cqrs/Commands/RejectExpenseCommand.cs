using MediatR;

namespace ExpenseFlow.Application.Cqrs.Commands
{
        public record RejectExpenseCommand(Guid ExpenseId, string Reason) : IRequest<Unit>;

}
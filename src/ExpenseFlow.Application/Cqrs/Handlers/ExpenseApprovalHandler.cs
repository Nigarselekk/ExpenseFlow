// ExpenseFlow.Application/Cqrs/Handlers/ExpenseApprovalHandler.cs
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Application.Cqrs.Handlers
{
    public class ExpenseApprovalHandler :
        
        IRequestHandler<ApproveExpenseCommand, Unit>,
        IRequestHandler<RejectExpenseCommand, Unit>
    {
        private readonly ExpenseFlowDbContext context;
        public ExpenseApprovalHandler(ExpenseFlowDbContext db) => context = db;

        public async Task<Unit> Handle(ApproveExpenseCommand cmd, CancellationToken ct)
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(e => e.Id == cmd.ExpenseId, ct)
                        ?? throw new KeyNotFoundException($"Expense {cmd.ExpenseId} not found.");
            if (expense.Status != ExpenseStatus.Pending)
                throw new InvalidOperationException("Only Pending expenses can be approved.");

            expense.Status = ExpenseStatus.Approved;

            var account = await context.AccountInfos
                .Where(a => a.PersonnelId == expense.PersonnelId)
                .FirstOrDefaultAsync(ct)
                ?? throw new InvalidOperationException("Account not found for the personnel.");
            
            // EFT simülasyonu
            var payment = new PaymentTransaction {
                Id = Guid.NewGuid(),
                ExpenseId = expense.Id,
                AccountInfoId = account.Id,
                Amount = expense.Amount,
                TransactionDate = DateTime.UtcNow,
                TransferType = 0,                       // 0 = EFT
                Status = (TransactionStatus)1,          // 1 = Completed
                BankReference = $"EFT-{Guid.NewGuid():N}"
            };
            context.PaymentTransactions.Add(payment);

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        public async Task<Unit> Handle(RejectExpenseCommand cmd, CancellationToken ct)
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(e => e.Id == cmd.ExpenseId, ct)
                        ?? throw new KeyNotFoundException($"Expense {cmd.ExpenseId} not found.");
            if (expense.Status != ExpenseStatus.Pending)
                throw new InvalidOperationException("Only Pending expenses can be rejected.");

            expense.Status = ExpenseStatus.Rejected;
            expense.RejectReason = cmd.Reason;

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}


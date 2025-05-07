using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Application.Dtos.Reports;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Cqrs.Queries.Reports;

namespace ExpenseFlow.Application.Cqrs.Handlers.Reports;

    public class GetExpensesByCategoryHandler
        : IRequestHandler<GetExpensesByCategoryQuery, List<ExpenseByCategoryDto>>
    {
        private readonly ExpenseFlowDbContext _db;
        public GetExpensesByCategoryHandler(ExpenseFlowDbContext db) => _db = db;

        public async Task<List<ExpenseByCategoryDto>> Handle(
            GetExpensesByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var q = _db.Expenses
                .AsNoTracking()
                .Where(e => (!request.From.HasValue || e.Date >= request.From.Value)
                         && (!request.To  .HasValue || e.Date <= request.To.Value));

            var grouped = await q
                .GroupBy(e => new { e.CategoryId, e.ExpenseCategory.Name })
                .Select(g => new ExpenseByCategoryDto {
                    CategoryId   = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    TotalAmount  = g.Sum(x => x.Amount)
                })
                .ToListAsync(cancellationToken);

            return grouped;
        }
    }


using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Infrastructure.DbContext;

namespace ExpenseFlow.Application.Cqrs.Handlers
{
    public class ExpenseHandler :
        IRequestHandler<CreateExpenseCommand, ExpenseResponse>,
        IRequestHandler<UpdateExpenseCommand, Unit>,
        IRequestHandler<DeleteExpenseCommand, Unit>,
        IRequestHandler<GetAllExpensesQuery, List<ExpenseResponse>>,
        IRequestHandler<GetExpenseByIdQuery, ExpenseResponse>,
        IRequestHandler<GetExpensesByPersonnelQuery, List<ExpenseResponse>>
    {
        private const string NotFoundMsg = "{0} with Id {1} not found";
        private readonly ExpenseFlowDbContext _context;
        private readonly IMapper _mapper;

        public ExpenseHandler(ExpenseFlowDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        // CREATE
        public async Task<ExpenseResponse> Handle(CreateExpenseCommand command, CancellationToken ct)
        {
            await EnsureExistsAsync<Personnel>(
                _context.Personnels, 
                command.Request.PersonnelId, 
                nameof(Personnel), 
                ct);

            await EnsureExistsAsync<ExpenseCategory>(
                _context.ExpenseCategories, 
                command.Request.CategoryId, 
                nameof(ExpenseCategory), 
                ct);

            var entity = _mapper.Map<Expense>(command.Request);
            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync(ct);

            // return the full DTO from the freshly‐saved record
            return await ExpensesQuery()
                .Where(e => e.Id == entity.Id)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .SingleAsync(ct);
        }

        // UPDATE
        public async Task<Unit> Handle(UpdateExpenseCommand command, CancellationToken ct)
        {
            var entity = await _context.Expenses.FindAsync(new object[]{command.Id}, ct)
                         ?? throw new KeyNotFoundException(string.Format(NotFoundMsg, "Expense", command.Id));

            _mapper.Map(command.Request, entity);
            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // DELETE
        public async Task<Unit> Handle(DeleteExpenseCommand command, CancellationToken ct)
        {
            var entity = await _context.Expenses.FindAsync(new object[]{command.Id}, ct)
                         ?? throw new KeyNotFoundException(string.Format(NotFoundMsg, "Expense", command.Id));

            _context.Expenses.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // GET ALL
        public async Task<List<ExpenseResponse>> Handle(GetAllExpensesQuery req, CancellationToken ct) =>
            await ExpensesQuery()
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

        // GET BY ID
        public async Task<ExpenseResponse> Handle(GetExpenseByIdQuery req, CancellationToken ct) =>
            await ExpensesQuery()
                .Where(e => e.Id == req.Id)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(ct)
                ?? throw new KeyNotFoundException(string.Format(NotFoundMsg, "Expense", req.Id));

        // GET BY PERSONNEL
        public async Task<List<ExpenseResponse>> Handle(GetExpensesByPersonnelQuery req, CancellationToken ct) =>
            await ExpensesQuery()
                .Where(e => e.PersonnelId == req.PersonnelId)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

        // Shared query with all the includes
        private IQueryable<Expense> ExpensesQuery() =>
            _context.Expenses
                .Include(e => e.Personnel)
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Attachments)
                .Include(e => e.Transactions);

        // DRY validation helper
        private static async Task EnsureExistsAsync<TEntity>(
            DbSet<TEntity> set,
            object id,
            string entityName,
            CancellationToken ct
        ) where TEntity : class
        {
            var exists = await set.FindAsync(new object[]{ id }, ct) is not null;
            if (!exists)
                throw new KeyNotFoundException(string.Format(NotFoundMsg, entityName, id));
        }
    }
}


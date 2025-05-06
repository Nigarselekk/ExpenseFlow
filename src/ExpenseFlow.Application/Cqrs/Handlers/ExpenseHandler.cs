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
        private const string NotFound = "{0} with Id {1} not found";
        private readonly ExpenseFlowDbContext _context;
        private readonly IMapper _mapper;

        public ExpenseHandler(ExpenseFlowDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ExpenseResponse> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            await EnsureExistsAsync<Personnel>(
              _context.Personnels,
              command.PersonnelId,
              nameof(Personnel),
              cancellationToken);

            await EnsureExistsAsync<ExpenseCategory>(
              _context.ExpenseCategories,
              command.Request.CategoryId,
              nameof(ExpenseCategory),
              cancellationToken);

            var entity = _mapper.Map<Expense>(command.Request);
            entity.PersonnelId = command.PersonnelId;
            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return await ExpensesQuery()
              .Where(e => e.Id == entity.Id)
              .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
              .SingleAsync(cancellationToken);
        }


        public async Task<Unit> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Expenses.FindAsync(new object[] { command.Id }, cancellationToken)
                         ?? throw new KeyNotFoundException(string.Format(NotFound, "Expense", command.Id));

            _mapper.Map(command.Request, entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }


        public async Task<Unit> Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Expenses.FindAsync(new object[] { command.Id }, cancellationToken)
                         ?? throw new KeyNotFoundException(string.Format(NotFound, "Expense", command.Id));

            _context.Expenses.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }


        public async Task<List<ExpenseResponse>> Handle(GetAllExpensesQuery getAllExpensesQuery, CancellationToken cancellationToken) =>
            await ExpensesQuery()
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


        public async Task<ExpenseResponse> Handle(GetExpenseByIdQuery getExpenseByIdQuery, CancellationToken cancellationToken) =>
            await ExpensesQuery()
                .Where(e => e.Id == getExpenseByIdQuery.Id)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException(string.Format(NotFound, "Expense", getExpenseByIdQuery.Id));


        public async Task<List<ExpenseResponse>> Handle(GetExpensesByPersonnelQuery getExpensesByPersonnelQuery, CancellationToken cancellationToken) =>
            await ExpensesQuery()
                .Where(e => e.PersonnelId == getExpensesByPersonnelQuery.PersonnelId)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        // Shared query with all the includes
        private IQueryable<Expense> ExpensesQuery() =>
            _context.Expenses
                .Include(e => e.Personnel)
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Attachments)
                .Include(e => e.Transactions);

        // DRY validation helper method
        private static async Task EnsureExistsAsync<TEntity>(
            DbSet<TEntity> set,
            object id,
            string entityName,
            CancellationToken cancellationToken
        ) where TEntity : class
        {
            var exists = await set.FindAsync(new object[] { id }, cancellationToken) is not null;
            if (!exists)
                throw new KeyNotFoundException(string.Format(NotFound, entityName, id));
        }
    }
}